//using AspNetCore;
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
    public class TestController : Controller
    {
        private static ApplicationDbContext _context;
        public TestController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult TestInfo(int ID)
        {
            var test = _context.Tests.FirstOrDefault(t => t.Id == ID);

            var course = _context.Courses.FirstOrDefault(c => c.Id == test.CourseId);
            ViewBag.Course = course;

            return View(test);
        }

    }
}