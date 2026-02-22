using Microsoft.AspNetCore.Mvc;
using FitMatch.Models;
using FitMatch.Data;

namespace FitMatch.Controllers
{
    public class ProfileController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProfileController(AppDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: View User Profile
        [HttpGet]
        public async Task<IActionResult> Index(int? id)
        {
            var userId = id ?? HttpContext.Session.GetInt32("UserId");

            if (!userId.HasValue)
            {
                return RedirectToAction("Login", "Account");
            }

            var user = await Task.Run(() => _context.users.FirstOrDefault(u => u.Id == userId));

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: Edit Profile
        [HttpGet]
        public async Task<IActionResult> Edit()
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (!userId.HasValue)
            {
                return RedirectToAction("Login", "Account");
            }

            var user = await Task.Run(() => _context.users.FirstOrDefault(u => u.Id == userId));

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Update Profile
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(User model, IFormFile? profileImage)
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (!userId.HasValue || model.Id != userId)
            {
                return Unauthorized();
            }

            var user = await Task.Run(() => _context.users.FirstOrDefault(u => u.Id == userId));

            if (user == null)
            {
                return NotFound();
            }

            // Update user info
            user.Username = model.Username;
            user.Info = model.Info;

            // Handle profile image upload
            if (profileImage != null && profileImage.Length > 0)
            {
                try
                {
                    var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", "profiles");
                    Directory.CreateDirectory(uploadsFolder);

                    var fileName = $"{userId}_{DateTime.Now.Ticks}{Path.GetExtension(profileImage.FileName)}";
                    var filePath = Path.Combine(uploadsFolder, fileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await profileImage.CopyToAsync(fileStream);
                    }

                    user.ProfileUrl = $"/uploads/profiles/{fileName}";
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Error uploading file: {ex.Message}");
                    return View(user);
                }
            }

            _context.users.Update(user);
            await _context.SaveChangesAsync();

            // Update session
            HttpContext.Session.SetString("Username", user.Username);

            return RedirectToAction("Index");
        }
    }
}
