//using AspNetCore;
using DistanceEducation.Data;
using DistanceEducation.Models;
using DistanceEducation.Models.Acount;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Diagnostics;
using System.Security.Claims;
using System.Text.RegularExpressions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace DistanceEducation.Controllers
{
    public class CourseController : Controller
    {
        private static ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public CourseController(RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, ApplicationDbContext context)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var UserName = User.Identity.Name;
            var user = _context.Students.FirstOrDefault(n => n.UserName == UserName);
            List<GroupCourse> courses = _context.GroupCourse.Where(g => g.GroupId == user.GroupId).Include(c => c.Course).ToList();
           
            var lecturers = _context.LecturerCourse.Include(l => l.Lecturer).ToList();
            ViewBag.Lecturers = lecturers;


            return View(courses);
        }

        [HttpGet]
        public IActionResult Course(int ID)
        {
            
            var course = _context.Courses.FirstOrDefault(c => c.Id ==ID);
            var tests = _context.Tests.Where(t => t.CourseId == ID).ToList();
            ViewBag.Tests = tests;
            return View(course);
        }


             //<!--div class="row">
               // @Html.ActionLink(@item.Name, "Test", new { ID = @item.Id})
//            </div-->



    }
}
