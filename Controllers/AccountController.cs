using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MyWeb.Models;
using Microsoft.AspNetCore.Http;
using System.Linq;
using Microsoft.VisualBasic;
using System.ComponentModel.Design;
using System.Reflection.Metadata;

using Microsoft.EntityFrameworkCore;
namespace MyWeb.Controllers;

public class AccountController : Controller
{
    private readonly AppDbContext _db;

    public AccountController(AppDbContext db)
    {
        _db = db;
    }
    
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Login(string username, string password)
    {
        var user = _db.Users.FirstOrDefault(u => u.Username == username);

        if (user != null && BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
        {
            HttpContext.Session.SetString("user", username);
            return RedirectToAction("Index", "Home");
        }

        ViewBag.Error = "Invalid login";
        return View();
    }

    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Index","Home");
    }

    public IActionResult Register()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Register(User model)
    {
        if (ModelState.IsValid)
        {
            bool isUserExist = await _db.Users.AnyAsync(u =>
                u.Username == model.Username || u.Email == model.Email);

            if (isUserExist)
            {
                ViewBag.Error = "Username หรือ Email นี้ถูกใช้งานไปแล้ว";
                return View(model);
            }

            model.PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.PasswordHash);

            _db.Users.Add(model);
            await _db.SaveChangesAsync();

            return RedirectToAction("Login","Account");

        }
        return View(model);
    }
}