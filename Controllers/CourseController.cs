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
            if (User.IsInRole("Student"))
            {
                var UserName = User.Identity.Name;
                var user = _context.Students.FirstOrDefault(n => n.UserName == UserName);
                //List<Course> courses = _context.Courses.Include(g => g.GroupCourse);

                List<GroupCourse> groupcourses = _context.GroupCourse.Where(g => g.GroupId == user.GroupId).Include(c => c.Course).ToList();
                List<Course> courses = new List<Course>();
                foreach (GroupCourse i in groupcourses)
                {
                    courses.Add(i.Course);
                }
          
                var lecturers = _context.LecturerCourse.Include(l => l.Lecturer).ToList();
                ViewBag.Lecturers = lecturers;
                return View(courses);
            }
            else if (User.IsInRole("Lecturer"))
            {
                var UserName = User.Identity.Name;
                var user = _context.Lecturers.FirstOrDefault(n => n.UserName == UserName);
                List<Course> courses = _context.Courses.ToList();

                var lecturers = _context.LecturerCourse.Include(l => l.Lecturer).ToList();
                ViewBag.Lecturers = lecturers;
                return View(courses);
            }

            return View();

        }

        [HttpGet]
        public IActionResult Course(int ID)
        {
            bool editor = false;

            var course = _context.Courses.FirstOrDefault(c => c.Id == ID);
            var tests = _context.Tests.Where(t => t.CourseId == ID).ToList();
            ViewBag.Tests = tests;

            if (User.IsInRole("Lecturer"))
            {
                var user = _context.Lecturers.FirstOrDefault(n => n.UserName == User.Identity.Name);
                var editors = _context.LecturerCourse.Where(c => c.CourseId == course.Id).ToList();
                foreach (var i in editors)
                {
                    if (i.LecturerId == user.Id)
                    {
                        editor = true;
                        break;
                    }
                }
            }       

            ViewBag.isEditor = editor;

            return View(course);
        }


    }
}
