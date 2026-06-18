using ConnectFour.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ConnectFour.Data;

public class ApplicationDbContext : IdentityDbContext<AppUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Transaction> Transactions => Set<Transaction>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Budget> Budgets => Set<Budget>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<AppUser>(entity =>
        {
            entity.HasMany(u => u.Transactions)
                .WithOne(t => t.AppUser)
                .HasForeignKey(t => t.AppUserId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(u => u.Categories)
                .WithOne(c => c.AppUser)
                .HasForeignKey(c => c.AppUserId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(u => u.Budgets)
                .WithOne(b => b.AppUser)
                .HasForeignKey(b => b.AppUserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        builder.Entity<Category>(entity =>
        {
            entity.HasKey(c => c.Id);
            entity.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(100);

            entity.HasMany(c => c.Transactions)
                .WithOne(t => t.Category)
                .HasForeignKey(t => t.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasMany(c => c.Budgets)
                .WithOne(b => b.Category)
                .HasForeignKey(b => b.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        builder.Entity<Transaction>(entity =>
        {
            entity.HasKey(t => t.Id);
            entity.Property(t => t.Amount)
                .IsRequired()
                .HasPrecision(18, 2);
            entity.Property(t => t.Date)
                .IsRequired();
            entity.Property(t => t.Description)
                .IsRequired()
                .HasMaxLength(500);
            entity.Property(t => t.Type)
                .HasConversion<string>()
                .IsRequired();
        });

        builder.Entity<Budget>(entity =>
        {
            entity.HasKey(b => b.Id);
            entity.Property(b => b.Limit)
                .IsRequired()
                .HasPrecision(18, 2);
            entity.Property(b => b.Month)
                .IsRequired();
            entity.Property(b => b.Year)
                .IsRequired();
        });
    }
}
