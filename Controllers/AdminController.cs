//using AspNetCore;
using DistanceEducation.Data;
using DistanceEducation.Models;
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
        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
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

        [HttpPost]
        public async Task<IActionResult> Edit(int ID, User record)
        {
            try
            {
                va
                var seance = _context.Seance.Find(ID);
                seance.Date = record.Date;
                seance.HallId = record.HallId;
                seance.Name_Movie = record.Name_Movie;
                seance.Price = record.Price;
                seance.Time = record.Time;
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            catch
            {
                ViewData["Halls"] = new SelectList(_context.Set<Hall>(), "HallId", "HallId", record.HallId);
                return View(record);
            }



















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