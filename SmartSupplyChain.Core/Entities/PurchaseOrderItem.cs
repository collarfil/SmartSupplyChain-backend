using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartSupplyChain.Core.Common;

namespace SmartSupplyChain.Core.Entities
{
    public class PurchaseOrderItem:BaseAuditableEntity
    {
        public Guid Id { get; set; }
        public Guid PurchaseOrderId { get; set; }
        public Guid ProductId { get; set; }

        public int Quantity { get; set; }
        public decimal UnitCost { get; set; }

        // Navigation
        public PurchaseOrder PurchaseOrder { get; set; }
        public Product Product { get; set; }
    }
}
