using DistanceEducation.Data;
using DistanceEducation.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Security.Claims;

namespace DistanceEducation.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private static ApplicationDbContext _context;
        public HomeController(ApplicationDbContext context, ILogger<HomeController> logger)
        {
            _logger = logger;
            _context = context;
        }

        /**private static void UserInfo() {
            var res;
            if (User.IsInRole("Lecturer")) {
                res = _context.Lecturer.SingleOrDefault(e => e.UserId == HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)); 
            }
            if (User.IsInRole("Student")) {
                res = _context.Student.SingleOrDefault(e => e.UserId == HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)); 
            }
        }**/

        public IActionResult Index()
        {
            var user = _context.Users.SingleOrDefault(e => e.Id == HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
            UserInfo model = new UserInfo();
            //model.Name = user.Name;
            //model.Surname = user.Surname;   
            return View(model);
        }
        [Authorize(Roles = "Admin")]
        public IActionResult Privacy()
        {
            //var user = _context.Student.SingleOrDefault(e => e.UserId == HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}