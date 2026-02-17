using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartSupplyChain.Core.Enums
{
    public enum ShipmentStatus
    {
        PendingPickup = 0,
        InTransit = 1,
        Delayed = 2,
        Delivered = 3,
        Cancelled = 4
    }
}
