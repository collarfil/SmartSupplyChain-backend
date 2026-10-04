using System;

namespace SmartSupplyChain.Core.DTOs;

public record SensorReadingDto(
    Guid Id,
    Guid? VehicleId,
    string? VehicleRegistrationNumber,
    Guid? ShipmentId,
    string DeviceIdentifier,
    decimal Temperature,
    decimal Humidity,
    decimal Latitude,
    decimal Longitude,
    decimal Speed,
    DateTime RecordedAt
);

public record CreateSensorReadingDto(
    Guid? VehicleId,
    Guid? ShipmentId,
    string DeviceIdentifier,
    decimal Temperature,
    decimal Humidity,
    decimal Latitude,
    decimal Longitude,
    decimal Speed,
    DateTime RecordedAt
);