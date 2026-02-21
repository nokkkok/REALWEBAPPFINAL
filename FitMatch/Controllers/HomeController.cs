// using System.Diagnostics;
// using Microsoft.AspNetCore.Mvc;
// using project.Models;

// namespace project.Controllers;

// public class HomeController : Controller
// {
//     public IActionResult Index()
//     {
//         return View();
//     }

//     public IActionResult Privacy()
//     {
//         return View();
//     }

//     [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
//     public IActionResult Error()
//     {
//         return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
//     }
// }

using Microsoft.AspNetCore.Mvc;
using project.Models;

namespace project.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            var events = new List<Event>();

            for (int i = 1; i <= 9; i++)
            {
                events.Add(new Event
                {
                    Id = 1,
                    Name = "Morning Run" + i,
                    Location = "Bangsaen Beach",
                    EventDate = DateTime.Now.AddDays(i),
                    Description = "5KM community run" + i,
                    ImageUrl = "/images/run.jpg"
                });
            }

            return View(events);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Event model)
        {
            if (ModelState.IsValid)
            {
                return RedirectToAction("Index");
            }
            return View(model);
        }
    }
}

// public IActionResult Create()
// {
//     return View();
// }

// [HttpPost]
// [ValidateAntiForgeryToken]
// public IActionResult Create(Event model)
// {
//     if (Modelstate.IsValid)
//     {
//         return RedirectToAction("Index");
//     }
//     return View(model);
// }