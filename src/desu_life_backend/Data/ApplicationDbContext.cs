using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace desu_life_backend.Data;

public class ApplicationDbContext : IdentityDbContext<DesuLifeIdentityUser, IdentityRole<int>, int>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
        }
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
    }
}

// TODO: 根据实际情况增加IdentityUser（用户模型类）的字段，符合业务需求
public class DesuLifeIdentityUser : IdentityUser<int>
{
}