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
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace DistanceEducation.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private static ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public AdminController(RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, ApplicationDbContext context)
        {
            _roleManager = roleManager;
            _userManager = userManager;
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
            var groups =  _context.Groups.ToList();
            ViewBag.Groups = groups;
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

                    Student newStudnent = new Student();
                    newStudnent.Id = userId;
                    newStudnent.UserName = model.UserName;
                    newStudnent.Email = model.Email;
                    newStudnent.Name = model.Name;
                    newStudnent.Surname = model.Surname;
                    newStudnent.GroupId = model.GroupId;

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
        public async Task<IActionResult> DeleteStudent(string ID)
        {
                var user = await _userManager.FindByIdAsync(ID);
                if (user != null)
                {
                    var result = await _userManager.DeleteAsync(user);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Students", "Admin");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Ошибка при удалении пользователя.");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Пользователь не найден.");
                }

                return View();
        }
        [HttpGet]
        public IActionResult Students()
        {
            var listStudents = _context.Students.Include(g => g.Group);
            var groups = _context.Groups.ToList();
            ViewBag.Groups = groups;
            return View(listStudents);
        }
        [HttpGet]
        public async Task<IActionResult> StudentDetails(string ID)
        {
            var student = _context.Students.Include(g => g.Group).FirstOrDefault(s => s.Id.Equals(ID));
            Console.WriteLine(student.Id);

            return View(student);
        }

        [HttpGet]
        public async Task<IActionResult> EditStudent(string ID)
        {
            var student = _context.Students.FirstOrDefault(s => s.Id.Equals(ID));
            Console.WriteLine(student.Id);
            var groups = _context.Groups.ToList();
            ViewBag.Groups = groups;
            return View(student);
        }

        [HttpPost]
        public async Task<IActionResult> EditStudent(Student record)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(record.Id);
                user.UserName = record.UserName;
                user.Email = record.Email;
                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    /**Student newStudnent = new Student();
                    newStudnent.Id = record.Id;
                    newStudnent.UserName = record.UserName;
                    newStudnent.Email = record.Email;
                    newStudnent.Name = record.Name;
                    newStudnent.Surname = record.Surname;
                    newStudnent.RoleId = record.RoleId;**/

                    _context.Update(record);
                    _context.SaveChanges();

                    return RedirectToAction("Students", "Admin");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View();
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult RegisterLecturer()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterLecturer(RegisterLecturer model)
        {
            if (ModelState.IsValid)
            {
                var user = new IdentityUser { UserName = model.UserName, Email = model.Email };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "Lecturer");
                    var userId = await _userManager.GetUserIdAsync(user);
                    var userRole = await _userManager.GetRolesAsync(user);
                    var role = _roleManager.Roles.FirstOrDefault(r => r.Name == "Lecturer");

                    Lecturer newLecturer = new Lecturer();
                    newLecturer.Id = userId;
                    newLecturer.UserName = model.UserName;
                    newLecturer.Email = model.Email;
                    newLecturer.Name = model.Name;
                    newLecturer.Surname = model.Surname;
                    newLecturer.RoleId = role.Id;

                    _context.Add(newLecturer);
                    _context.SaveChanges();

                    return RedirectToAction("Lecturers", "Admin");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(model);
        }


        [HttpGet]
        public IActionResult Lecturers()
        {
            var listLecturers = _context.Lecturers.ToList();
            return View(listLecturers);
        }
        [HttpGet]
        public async Task<IActionResult> LecturerDetails(string ID)
        {
            var lecturer = _context.Lecturers.FirstOrDefault(s => s.Id.Equals(ID));
            return View(lecturer);
        }

        [HttpGet]
        public async Task<IActionResult> EditLecturer(string ID)
        {
            var lecturer = _context.Lecturers.FirstOrDefault(s => s.Id.Equals(ID));
            return View(lecturer);
        }

        [HttpPost]
        public async Task<IActionResult> EditLecturer(Lecturer record)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(record.Id);
                user.UserName = record.UserName;
                user.Email = record.Email;
                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    _context.Update(record);
                    _context.SaveChanges();

                    return RedirectToAction("Lecturers", "Admin");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View();
        }















        [HttpGet]
        public IActionResult GetStudentsByName(string name)
        {
            List<Student> students = null;
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
            List<Student> students = null;
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
        [HttpGet]
        public IActionResult GetStudentsByGroup(int groupid)
        {
            List<Student> students = null;
            if (groupid != 0)
            {
                students = _context.Students.Where(s => s.GroupId == groupid).Include(g => g.Group).ToList();
            }
            else
            {
                students = _context.Students.Include(g => g.Group).ToList();
            }
            return PartialView("ListStudents", students);
        }
        [HttpGet]
        public IActionResult GetLecturersByName(string name)
        {
            List<Lecturer> lectuders = null;
            if (name != null)
            {
                lectuders = _context.Lecturers.Where(s => s.Name.Contains(name)).ToList();
            }
            else
            {
                lectuders = _context.Lecturers.ToList();
            }
            return PartialView("ListLecturers", lectuders);
        }
        [HttpGet]
        public IActionResult GetLecturersBySurname(string surname)
        {
            List<Lecturer> lectuders = null;
            if (surname != null)
            {
                lectuders = _context.Lecturers.Where(s => s.Surname.Contains(surname)).ToList();
            }
            else
            {
                lectuders = _context.Lecturers.ToList();
            }
            return PartialView("ListLecturers", lectuders);
        }
    }
}
