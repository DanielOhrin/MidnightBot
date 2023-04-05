using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace MidnightBot.Data.BotAPI.Models;

public partial class MidnightSkyBotContext : DbContext
{
    public MidnightSkyBotContext()
    {
    }

    public MidnightSkyBotContext(DbContextOptions<MidnightSkyBotContext> options)
        : base(options)
    {
    }

    public virtual DbSet<UserProfile> UserProfiles { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder.UseSqlServer(new ConfigurationBuilder().AddUserSecrets("09ec827a-1c22-43fa-87a2-7c7d88d0d891").Build().GetConnectionString("DefaultConnection"));
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserProfile>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__UserProf__3214EC0717C7248E");

            entity.ToTable("UserProfile");

            entity.HasIndex(e => e.PlayerId, "UQ__UserProf__4A4E74C90EEFA337").IsUnique();

            entity.HasIndex(e => e.ApiKey, "UQ__UserProf__A4E6E18638BD1D3B").IsUnique();

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.ApiKey).HasMaxLength(30);
            entity.Property(e => e.IsBanned).HasDefaultValueSql("((0))");
            entity.Property(e => e.IsTrackingIsland).HasDefaultValueSql("((0))");
            entity.Property(e => e.PlayerId).HasMaxLength(30);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
