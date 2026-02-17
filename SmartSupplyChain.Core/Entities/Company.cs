using System;
using System.Collections.Generic;

namespace SmartSupplyChain.Core.Entities
{
    public class Company
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string RegistrationNumber { get; set; }
        public string Address { get; set; }
        public string ContactEmail { get; set; }
        public string ContactPhone { get; set; }

        // Navigation
        public ICollection<Warehouse> Warehouses { get; set; } = new List<Warehouse>();
        public ICollection<Driver> Drivers { get; set; } = new List<Driver>();
        public ICollection<Vehicle> Vehicles { get; set; } = new List<Vehicle>();
        public ICollection<Shipment> Shipments { get; set; } = new List<Shipment>();
    }
}
