using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace SmartSupplyChain.Infrastructure.Data;

public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();

        // Explicitly set the provider and connection string for design-time tooling
        optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=ApprovalSystemDb;Username=collinsfelix;Password=yourpassword");

        return new ApplicationDbContext(optionsBuilder.Options);
    }
}