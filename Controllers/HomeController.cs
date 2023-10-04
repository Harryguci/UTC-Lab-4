using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;
using UTC_LAB_4.Data;
using UTC_LAB_4.Models;

namespace UTC_LAB_4.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly SchoolContext _schoolContext;

        public HomeController(ILogger<HomeController> logger, SchoolContext context)
        {
            _logger = logger;
            _schoolContext = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Enrollment()
        {
            List<Enrollment> data = _schoolContext.Enrollments.ToList();
            return View(data);
        }

        public IActionResult Learner()
        {
            var data = _schoolContext.Learners.ToList();

            return View(data);
        }

        public IActionResult Major()
        {
            var data = _schoolContext.Majors.ToList();

            return View();
        }

        public IActionResult Course()
        {
            var data = _schoolContext.Courses.ToList();

            return View();
        }
    }
}