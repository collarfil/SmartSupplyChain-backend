using System;
using SmartSupplyChain.Core.Common;

namespace SmartSupplyChain.Core.Entities;

public class SensorReading : BaseAuditableEntity
{
    public Guid Id { get; set; }
    public Guid? VehicleId { get; set; }
    public Vehicle? Vehicle { get; set; }

    public Guid? ShipmentId { get; set; }
    public Shipment? Shipment { get; set; }

    public string DeviceIdentifier { get; set; } = string.Empty;
    public decimal Temperature { get; set; } // e.g., Celsius
    public decimal Humidity { get; set; }    // Percentage
    public decimal Latitude { get; set; }
    public decimal Longitude { get; set; }
    public decimal Speed { get; set; }       // km/h
    public DateTime RecordedAt { get; set; } = DateTime.UtcNow;
}