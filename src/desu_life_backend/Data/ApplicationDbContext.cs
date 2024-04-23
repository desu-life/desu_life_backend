﻿using desu_life_backend.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace desu_life_backend.Data;

public class ApplicationDbContext : IdentityDbContext<DesuLifeIdentityUser, IdentityRole<int>, int>
{
#nullable disable
    public DbSet<RefreshToken> RefreshTokens { get; set; }
#nullable enable

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