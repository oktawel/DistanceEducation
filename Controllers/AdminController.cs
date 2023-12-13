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

namespace DistanceEducation.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private static ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        public AdminController(RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, ApplicationDbContext context)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }


        [HttpGet]
        [AllowAnonymous]
        public IActionResult RegisterStudent()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterStudent(RegisterStudent model)
        {
            if (ModelState.IsValid)
            {
                var user = new IdentityUser { UserName = model.UserName, Email = model.Email };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {

                    await _userManager.AddToRoleAsync(user, "Student");
                    var userId = await _userManager.GetUserIdAsync(user);
                    var userRole = await _userManager.GetRolesAsync(user);
                    var role = _roleManager.Roles.FirstOrDefault(r => r.Name == "Student");

                    User newStudnent = new User();
                    newStudnent.Id = userId;
                    newStudnent.UserName = model.UserName;
                    newStudnent.Email = model.Email;
                    newStudnent.Name = model.Name;
                    newStudnent.Surname = model.Surname;
                    newStudnent.RoleId = role.Id;

                    _context.Add(newStudnent);
                    _context.SaveChanges();

                    return RedirectToAction("Students", "Admin");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(model);
        }


        [HttpGet]
        public IActionResult Students()
        {
            var listStudents = _context.Students.ToList();
            return View(listStudents);
        }
        [HttpGet]
        public async Task<IActionResult> StudentDetails(string ID)
        {
            var student = _context.Students.FirstOrDefault(s => s.Id.Equals(ID));
            Console.WriteLine(student.Id);

            return View(student);
        }

        /**[HttpPost]
        public async Task<IActionResult> Edit(int ID, User record)
        {

        }**/
















            [HttpGet]
        public IActionResult GetStudentsByName(string name)
        {
            List<User> students = null;
            if (name != null)
            {
                students = _context.Students.Where(s => s.Name.Contains(name)).ToList();
            }
            else
            {
                students = _context.Students.ToList();
            }
            return PartialView("ListStudents", students);
        }
        [HttpGet]
        public IActionResult GetStudentsBySurname(string surname)
        {
            List<User> students = null;
            if (surname != null)
            {
                students = _context.Students.Where(s => s.Surname.Contains(surname)).ToList();
            }
            else
            {
                students = _context.Students.ToList();
            }
            return PartialView("ListStudents", students);
        }
    }
}