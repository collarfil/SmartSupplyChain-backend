using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartSupplyChain.Infrastructure.Repository.IRepository;
using System.Linq.Expressions;
using SmartSupplyChain.Infrastructure.Data;

namespace SmartSupplyChain.Infrastructure.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<T> _dbSet;

        public Repository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        // -------------------
        // GET BY ID + INCLUDES
        // -------------------
        public async Task<T?> GetAsync(Guid id, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _dbSet;

            foreach (var include in includes)
                query = query.Include(include);

            return await query.FirstOrDefaultAsync(e => EF.Property<Guid>(e, "Id") == id);
        }

        // -------------------
        // GET ALL + INCLUDES
        // -------------------
        public async Task<IEnumerable<T>> GetAllAsync(params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _dbSet;

            foreach (var include in includes)
                query = query.Include(include);

            return await query.ToListAsync();
        }

        // -------------------
        // ADD
        // -------------------
        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        // -------------------
        // UPDATE
        // -------------------
        public void Update(T entity)
        {
            _dbSet.Update(entity);
        }

        // -------------------
        // REMOVE
        // -------------------
        public void Remove(T entity)
        {
            _dbSet.Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            _dbSet.RemoveRange(entities);
        }

        // -------------------
        // FIND WITH INCLUDE
        // -------------------
        public async Task<IEnumerable<T>> FindAsync(
            Expression<Func<T, bool>> predicate,
            params Expression<Func<T, object>>[] includes
        )
        {
            IQueryable<T> query = _dbSet.Where(predicate);

            foreach (var include in includes)
                query = query.Include(include);

            return await query.ToListAsync();
        }

        // -------------------
        // PAGINATION + FILTER + SEARCH + SORT
        // -------------------
        public async Task<(IEnumerable<T> Items, int TotalCount)> GetPagedAsync(
            int pageNumber,
            int pageSize,
            Expression<Func<T, bool>>? filter = null,
            Expression<Func<T, bool>>? search = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            params Expression<Func<T, object>>[] includes
        )
        {
            IQueryable<T> query = _dbSet;

            foreach (var include in includes)
                query = query.Include(include);

            if (filter != null)
                query = query.Where(filter);

            if (search != null)
                query = query.Where(search);

            int totalCount = await query.CountAsync();

            if (orderBy != null)
                query = orderBy(query);

            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }
    }

}