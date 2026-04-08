using EpsilonWebApp.Models;
using Microsoft.EntityFrameworkCore;

namespace EpsilonWebApp.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Customer> Customers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id)
                  .HasDefaultValueSql("NEWSEQUENTIALID()");  // better than NEWID() for index performance

            entity.Property(e => e.CompanyName)
                  .IsRequired()
                  .HasMaxLength(100);
        });
    }
}