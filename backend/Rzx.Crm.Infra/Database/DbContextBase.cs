using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Rzx.Crm.Infra.Configuration;
using Rzx.Crm.Core.Models;
using Rzx.Crm.Core.Exceptions;

namespace Rzx.Crm.Infra.Database
{
    public class DbContextBase : DbContext
    {
        protected DatabaseConfig databaseConfig;
        protected ILoggerFactory loggerFactory;

        public DbContextBase(DatabaseConfig databaseConfig, ILoggerFactory loggerFactory)
        {
            this.databaseConfig = databaseConfig;
            this.loggerFactory = loggerFactory;

            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging(true);
            optionsBuilder.UseLoggerFactory(loggerFactory);
        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Employee> Employees{ get; set; }
        public DbSet<Product> Products{ get; set; }
        public DbSet<Order> Orders{ get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>(entity =>
            {
                entity.HasKey(e => e.CustomerId).HasName("Cutomer_PK");
                entity.Property(e => e.CustomerId).ValueGeneratedOnAdd();
                entity.Property(e => e.FirstName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.LastName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.MiddleInitial).HasMaxLength(1);
                entity.Property(e => e.Timestamp).IsRequired().IsConcurrencyToken();
            });

            modelBuilder.Entity<Employee>(entity =>
            {
                entity.HasKey(e => e.EmployeeId).HasName("Employee_PK");
                entity.Property(e => e.EmployeeId).ValueGeneratedOnAdd();
                entity.Property(e => e.FirstName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.LastName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.MiddleInitial).HasMaxLength(1);
                entity.Property(e => e.Timestamp).IsRequired().IsConcurrencyToken();
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(e => e.ProductId).HasName("Product_PK");
                entity.Property(e => e.ProductId).ValueGeneratedOnAdd();
                entity.Property(e => e.Name).IsRequired().HasMaxLength(256);
                entity.Property(e => e.Timestamp).IsRequired().IsConcurrencyToken();
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(e => e.OrderId).HasName("Order_PK");
                entity.Property(e => e.OrderId).ValueGeneratedOnAdd();
                entity.Property(e => e.Timestamp).IsRequired().IsConcurrencyToken();
            });
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach(var entry in ChangeTracker.Entries().Where(e=>e.Entity is IEntity))
            {
                ((IEntity)entry.Entity).Timestamp = DateTime.UtcNow;
            }

            try
            {
                return await base.SaveChangesAsync(cancellationToken);
            }
            catch(Exception ex)
            {
                if (ex is DbUpdateConcurrencyException)
                {
                    throw new EntityStaleException(ChangeTracker.Entries().First().Entity as IEntity);
                }
                else
                    throw;
            }
        }
    }
}
