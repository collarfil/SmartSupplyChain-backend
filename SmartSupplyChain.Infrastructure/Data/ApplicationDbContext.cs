using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartSupplyChain.Core.Common;
using SmartSupplyChain.Core.Entities;

namespace SmartSupplyChain.Infrastructure.Data
{
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext>options): base(options)
        {
        }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Warehouse> Warehouses { get; set; }
        public DbSet<InventoryItem> InventoryItems { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<PurchaseOrder> PurchaseOrders { get; set; }
        public DbSet<PurchaseOrderItem> PurchaseOrderItems { get; set; }
        public DbSet<Driver> Drivers { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<Shipment> Shipments { get; set; }
        public DbSet<ShipmentItem> ShipmentItems { get; set; }
        public DbSet<RatePlan> RatePlans { get; set; }
        public DbSet<ShipmentTrackingEvent> ShipmentTrackingEvents { get; set; }
        public DbSet<IncidentReport> IncidentReports { get; set; }
        public DbSet<RateUsageRecord> RateUsageRecords { get; set; }
        public DbSet<SensorReading> SensorReadings { get; set; }
        public DbSet<SupplyChainAnomaly> SupplyChainAnomalies { get; set; }
        public DbSet<QualityInspection> QualityInspections { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<SalesOrder> SalesOrders { get; set; }
        public DbSet<SalesOrderItem> SalesOrderItems { get; set; }
        public DbSet<DemandForecast> DemandForecasts { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // 1. Sensor Reading Configuration
        modelBuilder.Entity<SensorReading>(builder =>
        {
            builder.HasOne(s => s.Vehicle)
                   .WithMany()
                   .HasForeignKey(s => s.VehicleId)
                   .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(s => s.Shipment)
                   .WithMany()
                   .HasForeignKey(s => s.ShipmentId)
                   .OnDelete(DeleteBehavior.SetNull);
        });

        // 2. Supply Chain Anomaly Configuration
        modelBuilder.Entity<SupplyChainAnomaly>(builder =>
        {
            builder.HasOne(a => a.Shipment)
                   .WithMany()
                   .HasForeignKey(a => a.ShipmentId)
                   .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(a => a.InventoryItem)
                   .WithMany()
                   .HasForeignKey(a => a.InventoryItemId)
                   .OnDelete(DeleteBehavior.SetNull);
        });

        // 3. Quality Inspection Configuration
        modelBuilder.Entity<QualityInspection>(builder =>
        {
            builder.HasOne(q => q.Supplier)
                   .WithMany()
                   .HasForeignKey(q => q.SupplierId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(q => q.PurchaseOrder)
                   .WithMany()
                   .HasForeignKey(q => q.PurchaseOrderId)
                   .OnDelete(DeleteBehavior.SetNull);
        });

        // 4. Customer & Sales Orders Configuration
        modelBuilder.Entity<Customer>(builder =>
        {
            builder.HasMany(c => c.SalesOrders)
                   .WithOne(so => so.Customer)
                   .HasForeignKey(so => so.CustomerId)
                   .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<SalesOrder>(builder =>
        {
            builder.HasMany(so => so.SalesOrderItems)
                   .WithOne(soi => soi.SalesOrder)
                   .HasForeignKey(soi => soi.SalesOrderId)
                   .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<SalesOrderItem>(builder =>
        {
            builder.HasOne(soi => soi.Product)
                   .WithMany()
                   .HasForeignKey(soi => soi.ProductId)
                   .OnDelete(DeleteBehavior.Restrict);
        });

        // 5. Demand Forecast Configuration
        modelBuilder.Entity<DemandForecast>(builder =>
        {
            builder.HasOne(df => df.Product)
                   .WithMany()
                   .HasForeignKey(df => df.ProductId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(df => df.Warehouse)
                   .WithMany()
                   .HasForeignKey(df => df.WarehouseId)
                   .OnDelete(DeleteBehavior.Cascade);
        });
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach (var entry in ChangeTracker.Entries<BaseAuditableEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedAt = DateTime.UtcNow;
                    break;
                case EntityState.Modified:
                    entry.Entity.UpdatedAt = DateTime.UtcNow;
                    break;
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }

    }
}
