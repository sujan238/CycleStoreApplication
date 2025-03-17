using UserAuthApiPg.Models;
using Microsoft.EntityFrameworkCore;

namespace UserAuthApiPg.Data
{
    public class AuthDbContext : DbContext
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<CycleBrand> CycleBrands { get; set; }
        public DbSet<CycleType> CycleTypes { get; set; }
        public DbSet<Cycle> Cycles { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Inventory> Inventories { get; set; }
        public DbSet<ShippingDetails> ShippingDetails { get; set; }
        public DbSet<DeliveryProof> DeliveryProofs { get; set; }
        public DbSet<PurchaseOrder> PurchaseOrders { get; set; }
        public DbSet<EmployeeLog> EmployeeLogs { get; set; }
        public DbSet<FinancialReport> FinancialReports { get; set; }
        public DbSet<ReturnRequest> ReturnRequests { get; set; }
        public DbSet<Payment> Payments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Session relationship
            modelBuilder.Entity<Session>()
                .HasOne(s => s.User)
                .WithMany()
                .HasForeignKey(s => s.UserId);

            // Cycle relationships
            modelBuilder.Entity<Cycle>()
                .HasOne(c => c.Brand)
                .WithMany(cb => cb.Cycles)
                .HasForeignKey(c => c.BrandId);

            modelBuilder.Entity<Cycle>()
                .HasOne(c => c.Type)
                .WithMany(ct => ct.Cycles)
                .HasForeignKey(c => c.TypeId);

            // Transaction relationships
            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.Customer)
                .WithMany(c => c.Transactions)
                .HasForeignKey(t => t.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.Employee)
                .WithMany(u => u.Transactions)
                .HasForeignKey(t => t.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict);

            // Transaction to ShippingDetails (one-to-one)
            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.ShippingDetails)
                .WithOne(sd => sd.Transaction)
                .HasForeignKey<ShippingDetails>(sd => sd.TransactionId)
                .OnDelete(DeleteBehavior.Cascade);

            // Transaction to DeliveryProof (one-to-one via ShippingDetails)
            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.DeliveryProof)
                .WithOne()
                .HasForeignKey<DeliveryProof>(dp => dp.ShippingId)
                .OnDelete(DeleteBehavior.Cascade);

            // OrderDetail relationships
            modelBuilder.Entity<OrderDetail>()
                .HasOne(od => od.Transaction)
                .WithMany(t => t.OrderDetails)
                .HasForeignKey(od => od.TransactionId);

            modelBuilder.Entity<OrderDetail>()
                .HasOne(od => od.Cycle)
                .WithMany(c => c.OrderDetails) // Corrected: c.OrderDetails is ICollection<OrderDetail>
                .HasForeignKey(od => od.CycleId);

            // Inventory relationship
            modelBuilder.Entity<Inventory>()
                .HasOne(i => i.Cycle)
                .WithMany(c => c.Inventories)
                .HasForeignKey(i => i.CycleId);

            // ShippingDetails relationship
            modelBuilder.Entity<ShippingDetails>()
                .HasOne(sd => sd.DeliveryEmployee)
                .WithMany()
                .HasForeignKey(sd => sd.DeliveryEmployeeId)
                .OnDelete(DeleteBehavior.Restrict);

            // Payment relationship
            modelBuilder.Entity<Payment>()
                .HasOne(p => p.Transaction)
                .WithMany(t => t.Payments)
                .HasForeignKey(p => p.TransactionId)
                .OnDelete(DeleteBehavior.Cascade);

            // DeliveryProof relationships
            modelBuilder.Entity<DeliveryProof>()
                .HasOne(dp => dp.ShippingDetails)
                .WithOne()
                .HasForeignKey<DeliveryProof>(dp => dp.ShippingId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<DeliveryProof>()
                .HasOne(dp => dp.Employee)
                .WithMany(u => u.DeliveryProofs)
                .HasForeignKey(dp => dp.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict);

            // ReturnRequest relationship
            modelBuilder.Entity<ReturnRequest>()
                .HasOne(rr => rr.Transaction)
                .WithMany(t => t.ReturnRequests)
                .HasForeignKey(rr => rr.TransactionId);

            // EmployeeLog relationship
            modelBuilder.Entity<EmployeeLog>()
                .HasOne(el => el.Employee)
                .WithMany(u => u.EmployeeLogs)
                .HasForeignKey(el => el.EmployeeId);

            // PurchaseOrder relationship
            modelBuilder.Entity<PurchaseOrder>()
                .HasOne(po => po.Cycle)
                .WithMany()
                .HasForeignKey(po => po.CycleId);
        }
    }
}