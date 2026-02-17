using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartSupplyChain.Core.Entities
{
    public class Supplier
    {
        public Guid Id { get; set; }

        public string Name { get; set; }
        public string ContactEmail { get; set; }
        public string ContactPhone { get; set; }

        // Navigation
        public ICollection<PurchaseOrder> PurchaseOrders { get; set; } = new List<PurchaseOrder>();
    }
}
