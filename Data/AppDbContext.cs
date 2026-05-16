using Microsoft.EntityFrameworkCore;
using SchoolSystem.Models;

namespace SchoolSystem.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("users");
                entity.Property(u => u.Id).HasColumnName("id");
                entity.Property(u => u.Username).HasColumnName("username");
                entity.Property(u => u.PasswordHash).HasColumnName("passwordhash");
                entity.Property(u => u.Role).HasColumnName("role").HasConversion<string>();
                entity.Property(u => u.FullName).HasColumnName("fullname");
                entity.Property(u => u.Email).HasColumnName("email");
                entity.Property(u => u.CreatedAt).HasColumnName("createdat");
                entity.HasIndex(u => u.Username).IsUnique();
            });
        }
    }
}
