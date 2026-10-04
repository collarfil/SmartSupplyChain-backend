using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace SmartSupplyChain.Infrastructure.Data;

public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();

        // Fix: Change ApprovalSystemDb to SmartSupplyChainDb
        optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=SmartSupplyChainDb;Username=collinsfelix;Password=");

        return new ApplicationDbContext(optionsBuilder.Options);
    }
}