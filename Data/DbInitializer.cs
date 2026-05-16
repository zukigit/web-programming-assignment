using SchoolSystem.Data;
using SchoolSystem.Models;
using SchoolSystem.Services;

namespace SchoolSystem.Data
{
    public static class DbInitializer
    {
        public static void Seed(AppDbContext db)
        {
            if (db.Users.Any())
                return;

            db.Users.AddRange(
                new User
                {
                    Username = "admin",
                    PasswordHash = AuthService.HashPassword("admin123"),
                    Role = UserRole.Admin,
                    FullName = "System Administrator",
                    Email = "admin@school.edu",
                    CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                },
                new User
                {
                    Username = "teacher1",
                    PasswordHash = AuthService.HashPassword("teacher123"),
                    Role = UserRole.Teacher,
                    FullName = "John Smith",
                    Email = "john.smith@school.edu",
                    CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                },
                new User
                {
                    Username = "student1",
                    PasswordHash = AuthService.HashPassword("student123"),
                    Role = UserRole.Student,
                    FullName = "Alice Johnson",
                    Email = "alice.j@school.edu",
                    CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                }
            );

            db.SaveChanges();
        }
    }
}
