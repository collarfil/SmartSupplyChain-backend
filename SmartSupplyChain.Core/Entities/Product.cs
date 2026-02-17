using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartSupplyChain.Core.Entities
{
    public class Product
    {
        public Guid Id { get; set; }
        public Guid WarehouseId { get; set; }

        public string Name { get; set; }
        public string SKU { get; set; }
        public decimal UnitCost { get; set; }
        public string Description { get; set; }

        // Navigation
        public Warehouse Warehouse { get; set; }
        public ICollection<ShipmentItem> ShipmentItems { get; set; } = new List<ShipmentItem>();
    }
}
