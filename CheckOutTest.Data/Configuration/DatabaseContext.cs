using CheckOutTest.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace CheckOutTest.Data.Configuration
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options)
             : base(options)
        {
        }
        public DbSet<Payment> Payments { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }
    }
}