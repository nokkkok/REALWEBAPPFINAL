using Microsoft.AspNetCore.Mvc;
using FitMatch.Models;
using FitMatch.Data;
using BCrypt.Net;

namespace FitMatch.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;

        public AccountController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Register page
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        // POST: Handle registration
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Check if user already exists
                var existingUser = _context.users.FirstOrDefault(u => u.Email == model.Email || u.Username == model.Username);
                if (existingUser != null)
                {
                    ModelState.AddModelError("", "Email or username already exists");
                    return View(model);
                }

                // Create new user
                var user = new User
                {
                    Username = model.Username,
                    Email = model.Email,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password)
                };

                _context.users.Add(user);
                await _context.SaveChangesAsync();

                // Redirect to login
                return RedirectToAction("Login", new { registered = true });
            }

            return View(model);
        }

        // GET: Login page
        [HttpGet]
        public IActionResult Login(bool registered = false)
        {
            if (registered)
            {
                ViewBag.Message = "Registration successful! Please log in.";
            }
            return View();
        }

        // POST: Handle login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _context.users.FirstOrDefault(u => u.Email == model.Email);

                if (user == null || !BCrypt.Net.BCrypt.Verify(model.Password, user.PasswordHash))
                {
                    ModelState.AddModelError("", "Invalid email or password");
                    return View(model);
                }

                // Set session
                HttpContext.Session.SetInt32("UserId", user.Id);
                HttpContext.Session.SetString("Username", user.Username);
                HttpContext.Session.SetString("Email", user.Email);

                return RedirectToAction("Index", "Home");
            }

            return View(model);
        }

        // GET: Logout
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }

    // ViewModels for Register/Login
    public class RegisterViewModel
    {
        public required string Username { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public required string ConfirmPassword { get; set; }
    }

    public class LoginViewModel
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
}
