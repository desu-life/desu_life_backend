using desu.life.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace desu.life.Data;

public class ApplicationDbContext : IdentityDbContext<DesuLifeIdentityUser, DesulifeIdentityRole, int>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
#nullable disable
    public DbSet<RefreshToken> RefreshTokens { get; set; }

    public DbSet<UserLink> UserLink { get; set; }

    public DbSet<RedeemCode> RedeemCodes { get; set; }

    public DbSet<UserLinkArchive> UserLinkArchive { get; set; }

#nullable enable

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

public DbSet<desu.life.Data.Models.InfoPanel> InfoPanel { get; set; } = default!;
}