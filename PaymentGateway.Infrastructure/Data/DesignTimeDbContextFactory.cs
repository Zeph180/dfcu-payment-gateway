using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace PaymentGateway.Infrastructure.Data;

public class DesignTimeDbContextFactory: IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionsBuilder.UseSqlServer("Server=172.16.2.4;Database=PaymentDb;User Id=sa;Password=Ben#Evn2nE;TrustServerCertificate=True;");

        return new AppDbContext(optionsBuilder.Options);
    }
}