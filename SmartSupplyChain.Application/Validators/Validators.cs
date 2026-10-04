using FluentValidation;
using SmartSupplyChain.Core.DTOs.Product;
using SmartSupplyChain.Core.DTOs.PurchaseOrder;
using SmartSupplyChain.Core.DTOs.RatePlan;
using SmartSupplyChain.Core.DTOs.RateUsageRecord;
using SmartSupplyChain.Core.DTOs.Shipment;
using SmartSupplyChain.Core.DTOs.Supplier;
using SmartSupplyChain.Core.DTOs.Vehicle;
using SmartSupplyChain.Core.DTOs.Warehouse;

namespace SmartSupplyChain.Application.Validators
{
    // ----------------------------------------------------
    // 1. Product Validators
    // ----------------------------------------------------
    public class CreateProductDtoValidator : AbstractValidator<CreateProductDto>
    {
        public CreateProductDtoValidator()
        {
            RuleFor(x => x.WarehouseId)
                .NotEmpty().WithMessage("Warehouse ID is required.");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Product name is required.")
                .MaximumLength(150).WithMessage("Product name cannot exceed 150 characters.");

            RuleFor(x => x.SKU)
                .NotEmpty().WithMessage("SKU is required.")
                .MaximumLength(50).WithMessage("SKU cannot exceed 50 characters.");

            RuleFor(x => x.UnitCost)
                .GreaterThan(0).WithMessage("Unit cost must be greater than zero.");

            RuleFor(x => x.Description)
                .MaximumLength(500).WithMessage("Description cannot exceed 500 characters.");
        }
    }

    public class UpdateProductDtoValidator : AbstractValidator<UpdateProductDto>
    {
        public UpdateProductDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Product name is required.")
                .MaximumLength(150).WithMessage("Product name cannot exceed 150 characters.");

            RuleFor(x => x.SKU)
                .NotEmpty().WithMessage("SKU is required.")
                .MaximumLength(50).WithMessage("SKU cannot exceed 50 characters.");

            RuleFor(x => x.UnitCost)
                .GreaterThan(0).WithMessage("Unit cost must be greater than zero.");

            RuleFor(x => x.Description)
                .MaximumLength(500).WithMessage("Description cannot exceed 500 characters.");
        }
    }

    // ----------------------------------------------------
    // 2. PurchaseOrder Validators
    // ----------------------------------------------------
    public class CreatePurchaseOrderItemDtoValidator : AbstractValidator<CreatePurchaseOrderItemDto>
    {
        public CreatePurchaseOrderItemDtoValidator()
        {
            RuleFor(x => x.ProductId)
                .NotEmpty().WithMessage("Product ID is required.");

            RuleFor(x => x.Quantity)
                .GreaterThan(0).WithMessage("Quantity must be greater than zero.");

            RuleFor(x => x.UnitCost)
                .GreaterThanOrEqualTo(0).WithMessage("Unit cost cannot be negative.");
        }
    }

    public class CreatePurchaseOrderDtoValidator : AbstractValidator<CreatePurchaseOrderDto>
    {
        public CreatePurchaseOrderDtoValidator()
        {
            RuleFor(x => x.SupplierId)
                .NotEmpty().WithMessage("Supplier ID is required.");

            RuleFor(x => x.Items)
                .NotEmpty().WithMessage("Purchase order must contain at least one item.");

            RuleForEach(x => x.Items)
                .SetValidator(new CreatePurchaseOrderItemDtoValidator());
        }
    }

    // ----------------------------------------------------
    // 3. RatePlan & RateUsageRecord Validators
    // ----------------------------------------------------
    public class CreateRatePlanDtoValidator : AbstractValidator<CreateRatePlanDto>
    {
        public CreateRatePlanDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Rate plan name is required.")
                .MaximumLength(100).WithMessage("Name cannot exceed 100 characters.");

            RuleFor(x => x.CostPerKm)
                .GreaterThanOrEqualTo(0).WithMessage("Cost per Km cannot be negative.");

            RuleFor(x => x.CostPerKg)
                .GreaterThanOrEqualTo(0).WithMessage("Cost per Kg cannot be negative.");
        }
    }

    public class UpdateRatePlanDtoValidator : AbstractValidator<UpdateRatePlanDto>
    {
        public UpdateRatePlanDtoValidator()
        {
            Include(new CreateRatePlanDtoValidator());
        }
    }

  public class CreateRateUsageRecordDtoValidator : AbstractValidator<CreateRateUsageRecordDto>
    {
        public CreateRateUsageRecordDtoValidator()
        {
            RuleFor(x => x.RatePlanId)
                .NotEmpty().WithMessage("Rate plan ID is required.");

            RuleFor(x => x.ShipmentId)
                .NotEmpty().WithMessage("Shipment ID is required.");

            RuleFor(x => x.TotalDistanceKm)
                .GreaterThan(0).WithMessage("Total distance must be greater than zero.");

            RuleFor(x => x.TotalWeightKg)
                .GreaterThan(0).WithMessage("Total weight must be greater than zero.");
        }
    }

    // ----------------------------------------------------
    // 4. Shipment Validators
    // ----------------------------------------------------
    public class CreateShipmentItemDtoValidator : AbstractValidator<CreateShipmentItemDto>
    {
        public CreateShipmentItemDtoValidator()
        {
            RuleFor(x => x.ProductId)
                .NotEmpty().WithMessage("Product ID is required.");

            RuleFor(x => x.Quantity)
                .GreaterThan(0).WithMessage("Quantity must be greater than zero.");
        }
    }

    public class CreateShipmentDtoValidator : AbstractValidator<CreateShipmentDto>
    {
        public CreateShipmentDtoValidator()
        {
            RuleFor(x => x.CompanyId).NotEmpty().WithMessage("Company ID is required.");
            RuleFor(x => x.DriverId).NotEmpty().WithMessage("Driver ID is required.");
            RuleFor(x => x.VehicleId).NotEmpty().WithMessage("Vehicle ID is required.");
            RuleFor(x => x.Status).NotEmpty().WithMessage("Status is required.");

            RuleFor(x => x.Items)
                .NotEmpty().WithMessage("Shipment must contain at least one item.");

            RuleForEach(x => x.Items)
                .SetValidator(new CreateShipmentItemDtoValidator());
        }
    }

    public class CreateShipmentTrackingEventDtoValidator : AbstractValidator<CreateShipmentTrackingEventDto>
    {
        public CreateShipmentTrackingEventDtoValidator()
        {
            RuleFor(x => x.EventType)
                .NotEmpty().WithMessage("Event type is required.")
                .MaximumLength(50).WithMessage("Event type cannot exceed 50 characters.");

            RuleFor(x => x.Location)
                .NotEmpty().WithMessage("Location is required.")
                .MaximumLength(200).WithMessage("Location cannot exceed 200 characters.");
        }
    }

    // ----------------------------------------------------
    // 5. Supplier Validators
    // ----------------------------------------------------
    public class CreateSupplierDtoValidator : AbstractValidator<CreateSupplierDto>
    {
        public CreateSupplierDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Supplier name is required.")
                .MaximumLength(150).WithMessage("Supplier name cannot exceed 150 characters.");

            RuleFor(x => x.ContactEmail)
                .NotEmpty().WithMessage("Email address is required.")
                .EmailAddress().WithMessage("A valid email address is required.");

            RuleFor(x => x.ContactPhone)
                .NotEmpty().WithMessage("Phone number is required.")
                .Matches(@"^\+?[1-9]\d{1,14}$").WithMessage("Please provide a valid phone number.");
        }
    }

    // ----------------------------------------------------
    // 6. Vehicle Validators
    // ----------------------------------------------------
    public class CreateVehicleDtoValidator : AbstractValidator<CreateVehicleDto>
    {
        public CreateVehicleDtoValidator()
        {
            RuleFor(x => x.CompanyId).NotEmpty().WithMessage("Company ID is required.");

            RuleFor(x => x.PlateNumber)
                .NotEmpty().WithMessage("Plate number is required.")
                .MaximumLength(20).WithMessage("Plate number cannot exceed 20 characters.");

            RuleFor(x => x.Model)
                .NotEmpty().WithMessage("Vehicle model is required.");

            RuleFor(x => x.Year)
                .InclusiveBetween(1900, DateTime.UtcNow.Year + 1)
                .WithMessage("Please enter a valid manufacturing year.");
        }
    }

    // ----------------------------------------------------
    // 7. Warehouse Validators
    // ----------------------------------------------------
    public class CreateWarehouseDtoValidator : AbstractValidator<CreateWarehouseDto>
    {
        public CreateWarehouseDtoValidator()
        {
            RuleFor(x => x.CompanyId).NotEmpty().WithMessage("Company ID is required.");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Warehouse name is required.")
                .MaximumLength(150).WithMessage("Warehouse name cannot exceed 150 characters.");

            RuleFor(x => x.Location)
                .NotEmpty().WithMessage("Warehouse location is required.")
                .MaximumLength(250).WithMessage("Location cannot exceed 250 characters.");
        }
    }
}