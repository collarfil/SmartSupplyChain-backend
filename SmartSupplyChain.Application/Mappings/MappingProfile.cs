using System.Linq;
using AutoMapper;
using SmartSupplyChain.Core.DTOs.Product;
using SmartSupplyChain.Core.DTOs.PurchaseOrder;
using SmartSupplyChain.Core.DTOs.RatePlan;
using SmartSupplyChain.Core.DTOs.RateUsageRecord;
using SmartSupplyChain.Core.DTOs.Shipment;
using SmartSupplyChain.Core.DTOs.Supplier;
using SmartSupplyChain.Core.DTOs.Vehicle;
using SmartSupplyChain.Core.DTOs.Warehouse;
using SmartSupplyChain.Core.Entities;

namespace SmartSupplyChain.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // ----------------------------------------------------
            // 1. Product Mappings
            // ----------------------------------------------------
            CreateMap<CreateProductDto, Product>();
            CreateMap<UpdateProductDto, Product>();
            CreateMap<Product, ProductResponseDto>()
                .ForMember(dest => dest.WarehouseName, opt => opt.MapFrom(src => src.Warehouse != null ? src.Warehouse.Name : string.Empty));

            // ----------------------------------------------------
            // 2. PurchaseOrder & Items Mappings
            // ----------------------------------------------------
            CreateMap<CreatePurchaseOrderItemDto, PurchaseOrderItem>();
            CreateMap<PurchaseOrderItem, PurchaseOrderItemResponseDto>()
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product != null ? src.Product.Name : string.Empty))
                .ForMember(dest => dest.SKU, opt => opt.MapFrom(src => src.Product != null ? src.Product.SKU : string.Empty));

            CreateMap<CreatePurchaseOrderDto, PurchaseOrder>();
            CreateMap<UpdatePurchaseOrderStatusDto, PurchaseOrder>();
            CreateMap<PurchaseOrder, PurchaseOrderResponseDto>()
                .ForMember(dest => dest.SupplierName, opt => opt.MapFrom(src => src.Supplier != null ? src.Supplier.Name : string.Empty))
                .ForMember(dest => dest.TotalAmount, opt => opt.MapFrom(src => src.Items.Sum(i => i.Quantity * i.UnitCost)))
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items));

            // ----------------------------------------------------
            // 3. RatePlan & UsageRecord Mappings
            // ----------------------------------------------------
            CreateMap<CreateRatePlanDto, RatePlan>();
            CreateMap<UpdateRatePlanDto, RatePlan>();
            CreateMap<RatePlan, RatePlanResponseDto>();

            CreateMap<CreateRateUsageRecordDto, RateUsageRecord>();
            CreateMap<RateUsageRecord, RateUsageRecordResponseDto>()
                .ForMember(dest => dest.RatePlanName, opt => opt.MapFrom(src => src.RatePlan != null ? src.RatePlan.Name : string.Empty));

            // ----------------------------------------------------
            // 4. Shipment, Items & Tracking Mappings
            // ----------------------------------------------------
            CreateMap<CreateShipmentItemDto, ShipmentItem>();
            CreateMap<ShipmentItem, ShipmentItemResponseDto>()
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product != null ? src.Product.Name : string.Empty))
                .ForMember(dest => dest.SKU, opt => opt.MapFrom(src => src.Product != null ? src.Product.SKU : string.Empty));

            CreateMap<CreateShipmentTrackingEventDto, ShipmentTrackingEvent>();
            CreateMap<ShipmentTrackingEvent, ShipmentTrackingEventResponseDto>();

            CreateMap<CreateShipmentDto, Shipment>();
            CreateMap<UpdateShipmentStatusDto, Shipment>();
            CreateMap<Shipment, ShipmentResponseDto>()
                .ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src.Company != null ? src.Company.Name : string.Empty))
                .ForMember(dest => dest.DriverName, opt => opt.MapFrom(src => src.Driver != null ? src.Driver.FullName : string.Empty))
                .ForMember(dest => dest.VehiclePlateNumber, opt => opt.MapFrom(src => src.Vehicle != null ? src.Vehicle.PlateNumber : string.Empty))
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items))
                .ForMember(dest => dest.TrackingEvents, opt => opt.MapFrom(src => src.TrackingEvents));

            // ----------------------------------------------------
            // 5. Supplier Mappings
            // ----------------------------------------------------
            CreateMap<CreateSupplierDto, Supplier>();
            CreateMap<UpdateSupplierDto, Supplier>();
            CreateMap<Supplier, SupplierResponseDto>();

            // ----------------------------------------------------
            // 6. Vehicle Mappings
            // ----------------------------------------------------
            CreateMap<CreateVehicleDto, Vehicle>();
            CreateMap<UpdateVehicleDto, Vehicle>();
            CreateMap<Vehicle, VehicleResponseDto>()
                .ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src.Company != null ? src.Company.Name : string.Empty));

            // ----------------------------------------------------
            // 7. Warehouse Mappings
            // ----------------------------------------------------
            CreateMap<CreateWarehouseDto, Warehouse>();
            CreateMap<UpdateWarehouseDto, Warehouse>();
            CreateMap<Warehouse, WarehouseResponseDto>()
                .ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src.Company != null ? src.Company.Name : string.Empty));
        }
    }
}