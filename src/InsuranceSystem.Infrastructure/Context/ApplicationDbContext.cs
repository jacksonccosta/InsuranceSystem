using Microsoft.EntityFrameworkCore;
using InsuranceSystem.Domain.Entities;

namespace InsuranceSystem.Infrastructure.Context;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<VehicleInsurance> Insurances { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<VehicleInsurance>(builder =>
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.VehicleValue).HasPrecision(18, 2);
            builder.Property(x => x.RiskRate).HasPrecision(18, 4);
            builder.Property(x => x.RiskPremium).HasPrecision(18, 2);
            builder.Property(x => x.PurePremium).HasPrecision(18, 2);
            builder.Property(x => x.CommercialPremium).HasPrecision(18, 2);
        });
    }
}