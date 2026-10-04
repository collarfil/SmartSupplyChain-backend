using System;
using System.Threading;
using System.Threading.Tasks;
using SmartSupplyChain.Core.Entities;
using SmartSupplyChain.Core.Interface;
using SmartSupplyChain.Infrastructure.Data;

namespace SmartSupplyChain.Infrastructure.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _db;

        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));

            Companies = new Repository<Company>(_db);
            Drivers = new Repository<Driver>(_db);
            IncidentReports = new Repository<IncidentReport>(_db);
            InventoryItems = new Repository<InventoryItem>(_db);
            Products = new Repository<Product>(_db);
            PurchaseOrders = new Repository<PurchaseOrder>(_db);
            PurchaseOrderItems = new Repository<PurchaseOrderItem>(_db);
            RatePlans = new Repository<RatePlan>(_db);
            RateUsageRecords = new Repository<RateUsageRecord>(_db);
            Shipments = new Repository<Shipment>(_db);
            ShipmentItems = new Repository<ShipmentItem>(_db);
            ShipmentTrackingEvents = new Repository<ShipmentTrackingEvent>(_db);
            Suppliers = new Repository<Supplier>(_db);
            Vehicles = new Repository<Vehicle>(_db);
            Warehouses = new Repository<Warehouse>(_db);
        }

        public IRepository<Company> Companies { get; private set; }
        public IRepository<Driver> Drivers { get; private set; }
        public IRepository<IncidentReport> IncidentReports { get; private set; }
        public IRepository<InventoryItem> InventoryItems { get; private set; }
        public IRepository<Product> Products { get; private set; }
        public IRepository<PurchaseOrder> PurchaseOrders { get; private set; }
        public IRepository<PurchaseOrderItem> PurchaseOrderItems { get; private set; }
        public IRepository<RatePlan> RatePlans { get; private set; }
        public IRepository<RateUsageRecord> RateUsageRecords { get; private set; }
        public IRepository<Shipment> Shipments { get; private set; }
        public IRepository<ShipmentItem> ShipmentItems { get; private set; }
        public IRepository<ShipmentTrackingEvent> ShipmentTrackingEvents { get; private set; }
        public IRepository<Supplier> Suppliers { get; private set; }
        public IRepository<Vehicle> Vehicles { get; private set; }
        public IRepository<Warehouse> Warehouses { get; private set; }

        public async Task<int> CompleteAsync(CancellationToken cancellationToken = default)
        {
            return await _db.SaveChangesAsync(cancellationToken);
        }

        public async Task<bool> Commit()
        {
            return await _db.SaveChangesAsync() > 0;
        }

        public void Dispose()
        {
            _db.Dispose();
        }
    }
}