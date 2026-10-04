using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartSupplyChain.Core.Common;

namespace SmartSupplyChain.Core.Entities
{
    public class PurchaseOrder:BaseAuditableEntity
    {
        public Guid Id { get; set; }
        public Guid SupplierId { get; set; }

        public DateTime OrderDate { get; set; }
        public string Status { get; set; }

        // Navigation
        public Supplier Supplier { get; set; }
        public ICollection<PurchaseOrderItem> Items { get; set; } = new List<PurchaseOrderItem>();
    }
}
