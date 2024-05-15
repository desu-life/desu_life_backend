﻿using desu.life.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

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
}