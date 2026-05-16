using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using SchoolSystem.Data;
using SchoolSystem.Models;

namespace SchoolSystem.Services
{
    public class AuthService
    {
        private readonly AppDbContext _db;
        private readonly IHttpContextAccessor _http;

        public AuthService(AppDbContext db, IHttpContextAccessor http)
        {
            _db = db;
            _http = http;
        }

        public static string HashPassword(string password)
        {
            var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(bytes);
        }

        public async Task<User?> ValidateUser(string username, string password)
        {
            var hash = HashPassword(password);
            return await _db.Users.FirstOrDefaultAsync(u => u.Username == username && u.PasswordHash == hash);
        }

        public async Task SignIn(User user)
        {
            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new(ClaimTypes.Name, user.Username),
                new(ClaimTypes.Role, user.Role.ToString()),
                new("FullName", user.FullName)
            };

            var identity = new ClaimsIdentity(claims, "SchoolSystemCookie");
            var principal = new ClaimsPrincipal(identity);

            var context = _http.HttpContext!;
            await context.SignInAsync("SchoolSystemCookie", principal, new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddHours(2)
            });
        }

        public async Task SignOut()
        {
            var context = _http.HttpContext!;
            await context.SignOutAsync("SchoolSystemCookie");
        }
    }
}
