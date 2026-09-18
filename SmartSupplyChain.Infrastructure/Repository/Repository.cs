using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;    
using Microsoft.EntityFrameworkCore;
using SmartSupplyChain.Core.Interface;
using SmartSupplyChain.Core.Entities;
using SmartSupplyChain.Infrastructure.Data;

namespace SmartSupplyChain.Infrastructure.Repository 
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly ApplicationDbContext _db;
        internal readonly DbSet<TEntity> _dbSet;

        public Repository(ApplicationDbContext db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
            _dbSet = db.Set<TEntity>();
        }

        public async Task<TEntity?> GetByIdAsync(int id, params Expression<Func<TEntity, object>>[] includes)
        {
            if (includes != null && includes.Length > 0)
            {
                IQueryable<TEntity> query = _dbSet.AsNoTracking();
                query = ApplyIncludes(query, includes);

                return await query.FirstOrDefaultAsync(e => EF.Property<int>(e, "Id") == id);
            }

            return await _dbSet.FindAsync(id);
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync(params Expression<Func<TEntity, object>>[] includes)
        {
            IQueryable<TEntity> query = _dbSet.AsNoTracking();
            query = ApplyIncludes(query, includes);

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<TEntity>> FindAsync(
            Expression<Func<TEntity, bool>> predicate, 
            params Expression<Func<TEntity, object>>[] includes)
        {
            IQueryable<TEntity> query = _dbSet.AsNoTracking().Where(predicate);
            query = ApplyIncludes(query, includes);

            return await query.ToListAsync();
        }

        public async Task AddAsync(TEntity entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public async Task AddRangeAsync(IEnumerable<TEntity> entities)
        {
            await _dbSet.AddRangeAsync(entities);
        }

        public void Update(TEntity entity)
        {
            _dbSet.Update(entity);
        }

        public void Remove(TEntity entity)
        {
            _dbSet.Remove(entity);
        }

        public void RemoveRange(IEnumerable<TEntity> entities)
        {
            _dbSet.RemoveRange(entities);
        }

        private static IQueryable<TEntity> ApplyIncludes(
            IQueryable<TEntity> query, 
            Expression<Func<TEntity, object>>[] includes)
        {
            if (includes != null && includes.Length > 0)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }

            return query;
        }
    }
}