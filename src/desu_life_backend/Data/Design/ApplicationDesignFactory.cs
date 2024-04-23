using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace desu.life.Data.Design;

public class ApplicationDesignFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();

        optionsBuilder.UseMySql(ServerVersion.AutoDetect(""));

        return new ApplicationDbContext(optionsBuilder.Options);
    }
}