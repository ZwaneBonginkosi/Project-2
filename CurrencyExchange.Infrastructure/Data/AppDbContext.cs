using CurrencyExchange.ApplicationCore.Entities;
using Microsoft.EntityFrameworkCore;

namespace CurrencyExchange.Infrastructure.Data;

public class AppDbContext: DbContext
{
    public virtual DbSet<ConversionHistory> RatesHistory { get; set; }
    public virtual DbSet<CurrencyPairEntity> CurrencyPairEntity { get; set; }
    

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<ConversionHistory>(builder =>
        {
            builder.ToTable("ConversionHistory", "CurrencyExchange");
            
            builder.Property(e => e.Id)
                .HasColumnName("RatesHistoryId");
            builder.Property(e => e.Base)
                .HasColumnName("Base")
                .HasMaxLength(3);
            builder.Property(e => e.Target)
                .HasColumnName("Target")
                .HasMaxLength(3);
            builder.Property(e => e.Created)
                .HasColumnType("datetime");
            builder.Property(e => e.LastModified)
                .HasColumnType("datetime");
            builder.Property(e => e.Amount)
                .HasPrecision(15);
            builder.Property(e => e.TargetAmount)
                .HasPrecision(15);
        });

        modelBuilder.Entity<CurrencyPairEntity>(builder =>
        {
            builder.ToTable("CurrencyPair", "CurrencyExchange");
            
            builder.Property(e => e.Id)
                .HasColumnName("CurrencyPairId");
            builder.Property(e => e.Base)
                .HasColumnName("Base")
                .HasMaxLength(3);
            builder.Property(e => e.Target)
                .HasColumnName("Target")
                .HasMaxLength(3);
            builder.Property(e => e.Rate)
                .HasPrecision(15);
        });
    }
}
