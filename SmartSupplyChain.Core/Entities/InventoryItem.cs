using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartSupplyChain.Core.Common;

namespace SmartSupplyChain.Core.Entities
{
    public class InventoryItem:BaseAuditableEntity
    {
        public Guid Id { get; set; }
        public Guid WarehouseId { get; set; }
        public Guid ProductId { get; set; }

        public int QuantityAvailable { get; set; }
        public int ReorderLevel { get; set; }

        // Navigation
        public Warehouse Warehouse { get; set; }
        public Product Product { get; set; }
    }
}
