using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using SmartSupplyChain.Core.Entities;

namespace SmartSupplyChain.Core.Interface
{
    public interface IUnitOfWork:IDisposable
    {
        IRepository<Company> Companies { get; }
        IRepository<Driver> Drivers { get; }
        IRepository<IncidentReport> IncidentReports { get; }
        IRepository<InventoryItem> InventoryItems { get; }
        IRepository<Product> Products { get; }
        IRepository<PurchaseOrder> PurchaseOrders { get; }
        IRepository<PurchaseOrderItem> PurchaseOrderItems { get; }
        IRepository<RatePlan> RatePlans { get; }
        IRepository<RateUsageRecord> RateUsageRecords { get; }
        IRepository<Shipment> Shipments { get; }
      IRepository<ShipmentItem> ShipmentItems { get; }
        IRepository<ShipmentTrackingEvent> ShipmentTrackingEvents { get; }
        IRepository<Supplier> Suppliers { get; }
        IRepository<Vehicle> Vehicles { get; }
        IRepository<Warehouse> Warehouses { get; }
        Task<bool> Commit();
        Task<int> CompleteAsync();
    }
}