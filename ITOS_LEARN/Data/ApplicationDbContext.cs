using Microsoft.EntityFrameworkCore;
using ITOS_LEARN.Models;

namespace ITOS_LEARN.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Person> People { get; set; }
        public DbSet<ITOS_LEARN.Models.User> Users { get; set; }
        public DbSet<Login> Logins { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Login>()
                .Property(l => l.Id)
                .ValueGeneratedOnAdd(); // ให้ฐานข้อมูลจัดการการสร้าง Id
        }
    }
}