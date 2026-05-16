using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolSystem.Data;
using SchoolSystem.Models;
using SchoolSystem.Services;

namespace SchoolSystem.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly AppDbContext _db;

        public AdminController(AppDbContext db)
        {
            _db = db;
        }

        public IActionResult Dashboard()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Users()
        {
            var users = await _db.Users.OrderBy(u => u.Id).ToListAsync();
            return View(users);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new UserEditViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserEditViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            if (string.IsNullOrEmpty(model.Password))
            {
                ModelState.AddModelError("Password", "Password is required.");
                return View(model);
            }

            if (await _db.Users.AnyAsync(u => u.Username == model.Username))
            {
                ModelState.AddModelError("Username", "Username already exists.");
                return View(model);
            }

            var user = new User
            {
                Username = model.Username,
                PasswordHash = AuthService.HashPassword(model.Password),
                Role = model.Role,
                FullName = model.FullName,
                Email = model.Email,
                CreatedAt = DateTime.UtcNow
            };

            _db.Users.Add(user);
            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Users));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var user = await _db.Users.FindAsync(id);
            if (user == null)
                return NotFound();

            var model = new UserEditViewModel
            {
                Id = user.Id,
                Username = user.Username,
                Role = user.Role,
                FullName = user.FullName,
                Email = user.Email
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UserEditViewModel model)
        {
            if (id != model.Id)
                return BadRequest();

            if (!ModelState.IsValid)
                return View(model);

            var user = await _db.Users.FindAsync(id);
            if (user == null)
                return NotFound();

            if (await _db.Users.AnyAsync(u => u.Username == model.Username && u.Id != id))
            {
                ModelState.AddModelError("Username", "Username already exists.");
                return View(model);
            }

            user.Username = model.Username;
            user.Role = model.Role;
            user.FullName = model.FullName;
            user.Email = model.Email;

            if (!string.IsNullOrEmpty(model.Password))
                user.PasswordHash = AuthService.HashPassword(model.Password);

            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Users));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _db.Users.FindAsync(id);
            if (user == null)
                return NotFound();

            _db.Users.Remove(user);
            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Users));
        }
    }
}
