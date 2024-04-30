using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace desu.life.Data.Design;

public class ApplicationDesignFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();

        optionsBuilder.UseMySql(Program.connectionString, new MariaDbServerVersion(new Version(10, 11, 7)));

        return new ApplicationDbContext(optionsBuilder.Options);
    }
}