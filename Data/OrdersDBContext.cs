using Microsoft.EntityFrameworkCore;
using services.Models;

namespace Data
{
    public class OrdersDBContext: DbContext
    {
        public OrdersDBContext(DbContextOptions<OrdersDBContext> options) : base(options)
        {

        }

        public DbSet<Order> Order { get; set; }
        public DbSet<StateChange> StateChanges { get; set; }
        public DbSet<OrderState> States { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>()
                .HasKey(o => o.Id);

            modelBuilder.Entity<StateChange>()
                .HasKey(sc => sc.Id);

            modelBuilder.Entity<OrderState>()
                .HasKey(os => os.Id);

            modelBuilder.Entity<OrderState>()
                .Property(os => os.State)
                .IsRequired();
        }
    }
}