using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MyWeb.Models;
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace MyWeb.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        // if (HttpContext.Session.GetString("user") == null)
        //     return RedirectToAction("Login", "Account");

        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    public IActionResult New()
    {
        return View();
    }

    public IActionResult Loged()
    {
        if (string.IsNullOrEmpty(HttpContext.Session.GetString("user")))
            {
                return RedirectToAction("Login", "Account");
            }

        return View();
    }



    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
