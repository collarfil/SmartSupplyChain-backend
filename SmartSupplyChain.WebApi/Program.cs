using Microsoft.EntityFrameworkCore;
using SmartSupplyChain.Application.Services;
using SmartSupplyChain.Core.Interface;
using SmartSupplyChain.Core.Interface.Services;
using SmartSupplyChain.Core.Interfaces;
using SmartSupplyChain.Infrastructure.Data;
using SmartSupplyChain.Infrastructure.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("SmartSupplyChainDatabase")));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<ICompanyService, CompanyService>();
builder.Services.AddScoped<IDriverService, DriverService>();
builder.Services.AddScoped<IVehicleService, VehicleService>();
builder.Services.AddScoped<IPurchaseOrderService, PurchaseOrderService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IRatePlanService, RatePlanService>();
builder.Services.AddScoped<IRateUsageRecordService, RateUsageRecordService>();
builder.Services.AddScoped<IShipmentService, ShipmentService>();
builder.Services.AddScoped<ISupplierService, SupplierService>();
builder.Services.AddScoped<IWarehouseService, WarehouseService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
