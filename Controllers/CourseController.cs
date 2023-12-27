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
using System.Linq;
using System.Security.Claims;
using System.Text.RegularExpressions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static System.Net.Mime.MediaTypeNames;

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
            Console.WriteLine(ID);
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


        [HttpGet]
        public IActionResult AddTest(int courseId)
        {
            Console.WriteLine(courseId);
            ViewBag.Types = _context.QuestionTypes.ToList();
            ViewBag.Course = _context.Courses.FirstOrDefault(c => c.Id == courseId);
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddTest(AddTest model)
        {
            if (ModelState.IsValid)
            {
                //Console.WriteLine(model.DateStart);
                //Console.WriteLine(model.TimeStart);
                Test newTest = new Test();
                newTest.Name = model.Name;
                newTest.Description = model.Description;
                newTest.Length = model.Length;
                newTest.TimeStart = new DateTime(model.DateStart.Year,
                                                 model.DateStart.Month,
                                                 model.DateStart.Day,
                                                 model.TimeStart.Hour,
                                                 model.TimeStart.Minute,
                                                 0);
                newTest.TimeEnd = new DateTime(model.DateEnd.Year,
                                               model.DateEnd.Month,
                                               model.DateEnd.Day,
                                               model.TimeEnd.Hour,
                                               model.TimeEnd.Minute,
                                               0);
                newTest.CourseId = model.CourseId;

                Console.WriteLine(newTest.Id);
                Console.WriteLine(newTest.Name);
                Console.WriteLine(newTest.Description);
                Console.WriteLine(newTest.Length);
                Console.WriteLine(newTest.TimeStart);
                Console.WriteLine(newTest.TimeEnd);
                Console.WriteLine(newTest.CourseId);

                _context.Tests.Add(newTest);
                _context.SaveChanges();

                return RedirectToAction("Course", new { ID = model.CourseId}); ;
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult EditTest( int testId)
        {
            var test = _context.Tests.FirstOrDefault(t => t.Id == testId);
            ViewBag.Questions = _context.Questions.Where(q => q.TestId == testId).ToList();
            ViewBag.Types = _context.QuestionTypes.ToList();
            ViewBag.Course = _context.Courses.FirstOrDefault(c => c.Id == test.CourseId);

            AddTest testForm = new AddTest();
            testForm.Id = test.Id;
            testForm.CourseId = test.CourseId;
            testForm.Length = test.Length;
            testForm.Name = test.Name;
            testForm.Description = test.Description;
            testForm.DateStart = test.TimeStart;
            testForm.TimeStart = test.TimeStart;
            testForm.DateEnd = test.TimeEnd;
            testForm.TimeEnd = test.TimeEnd;

            return View(testForm);
        }
        [HttpPost]
        public async Task<IActionResult> EditTest(AddTest model)
        {
            Console.WriteLine(model.Id);
            Console.WriteLine(model.CourseId);
            Console.WriteLine(model.Length);
            Console.WriteLine(model.Name);
            Console.WriteLine(model.Description);
            Console.WriteLine(model.DateStart);
            Console.WriteLine(model.TimeStart);
            Console.WriteLine(model.DateEnd);
            Console.WriteLine(model.TimeEnd);


            if (ModelState.IsValid)
            {
                Test testForm = new Test();
                testForm.Id = model.Id;
                testForm.CourseId = model.CourseId;
                testForm.Length = model.Length;
                testForm.Name = model.Name;
                testForm.Description = model.Description;
                testForm.TimeStart = new DateTime(model.DateStart.Year,
                                                     model.DateStart.Month,
                                                     model.DateStart.Day,
                                                     model.TimeStart.Hour,
                                                     model.TimeStart.Minute,
                                                     0);
                testForm.TimeEnd = new DateTime(model.DateEnd.Year,
                                                   model.DateEnd.Month,
                                                   model.DateEnd.Day,
                                                   model.TimeEnd.Hour,
                                                   model.TimeEnd.Minute,
                                                   0);

                _context.Update(testForm);
                _context.SaveChanges();

                return RedirectToAction("Course", new { ID = model.CourseId }); ;
            }
            ViewBag.Questions = _context.Questions.Where(q => q.TestId == model.Id).ToList();
            ViewBag.Types = _context.QuestionTypes.ToList();
            ViewBag.Course = _context.Courses.FirstOrDefault(c => c.Id == model.CourseId);
            return View(model);
        }
        

        [HttpGet]
        public IActionResult EditQuestion(int questionId)
        {
            var question = _context.Questions.FirstOrDefault(t => t.Id == questionId);
            bool isMax = true;
            if (question.QuestionTypeId != 4)
            {
                ViewBag.OptionTF = false;
                ViewBag.Options = _context.Options.Where(o => o.QuestionId == questionId).ToList();
            }
            else if (question.QuestionTypeId == 1 && _context.Options.FirstOrDefault(o => o.QuestionId == question.Id) != null)
            {
                isMax = false;
            }
            else 
            {
                ViewBag.OptionTF = true;
                ViewBag.Options = _context.OptionTFQuestion.Where(o => o.QuestionId == questionId).Include(tf => tf.OptionTrueFalse).ToList();
            }
            
            ViewBag.isMax = isMax;

            ViewBag.Types = _context.QuestionTypes.ToList();

            AddQuestion questionForm = new AddQuestion();
            questionForm.Id = question.Id;
            questionForm.Name = question.Name;
            questionForm.QuestionTypeId = question.QuestionTypeId;
            questionForm.TestId = question.TestId;
            questionForm.Cost = question.Cost;

            return View(questionForm);
        }
        [HttpPost]
        public async Task<IActionResult> EditQuestion(AddQuestion model)
        {
            if (ModelState.IsValid)
            {
                Question quetionForm = new Question();
                quetionForm.Id = (int)model.Id;
                quetionForm.Name = model.Name;
                quetionForm.QuestionTypeId = model.QuestionTypeId;
                quetionForm.TestId = model.TestId;
                quetionForm.Cost = model.Cost;

                _context.Update(quetionForm);
                _context.SaveChanges();

                return RedirectToAction("EditTest", new { testId = model.TestId }); ;
            }
            return View(model);
        }



        [HttpGet]
        public IActionResult EditOption( int optionId)
        {
            var option = _context.Options.FirstOrDefault(o => o.Id == optionId);
            AddOption optionForm = new AddOption();
            optionForm.Id = option.Id;
            optionForm.Text = option.Text;
            optionForm.QuestionId = option.QuestionId;
            optionForm.Correct = option.Correct;         
            return View(optionForm);
        }
        [HttpPost]
        public async Task<IActionResult> EditOption(AddOption model)
        {
            if (ModelState.IsValid)
            {
                Option optionForm = new Option();
                optionForm.Id = (int)model.Id;
                optionForm.Text = model.Text;
                optionForm.QuestionId = model.QuestionId;
                optionForm.Correct = model.Correct;
                _context.Update(optionForm);
                _context.SaveChanges();
                return RedirectToAction("EditQuestion", new { questionId = model.QuestionId });
            }
            return View(model);
        }



        [HttpGet]
        public IActionResult AddQuestion(int testId)
        {
            Console.WriteLine(testId);
            ViewBag.Types = _context.QuestionTypes.ToList();
            ViewBag.Test = _context.Tests.FirstOrDefault(t => t.Id == testId);
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddQuestion(AddQuestion model)
        {
            if (ModelState.IsValid)
            {
                Question quetionForm = new Question();
                quetionForm.Name = model.Name;
                quetionForm.QuestionTypeId = model.QuestionTypeId;
                quetionForm.TestId = model.TestId;
                quetionForm.Cost = model.Cost;

                _context.Add(quetionForm);
                _context.SaveChanges();

                return RedirectToAction("EditTest", new { testId = model.TestId }); ;
            }
            return View(model);
        }



        [HttpGet]
        public IActionResult AddOption(int questionId)
        {
            Console.WriteLine(questionId);
            var question = _context.Questions.FirstOrDefault(t => t.Id == questionId);
            if ( question.QuestionTypeId == 4)
            {
                return RedirectToAction("AddTFOption", new { questionId = questionId }); ;
            }
            ViewBag.Question = question;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddOption(AddOption model)
        {
            if (ModelState.IsValid)
            {
                Option optionForm = new Option();
                optionForm.Text = model.Text;
                optionForm.QuestionId = model.QuestionId;
                optionForm.Correct = model.Correct;

                _context.Add(optionForm);
                _context.SaveChanges();

                return RedirectToAction("EditQuestion", new { questionId = model.QuestionId }); ;
            }
            return View(model);
        }




        [HttpGet]
        public IActionResult AddTFOption(int questionId)
        {
            Console.WriteLine(questionId);
            ViewBag.Question = _context.Questions.FirstOrDefault(t => t.Id == questionId); ;

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddTFOption(AddOptionTrueFalse model)
        {
            if (ModelState.IsValid)
            {
                OptionTFQuestion optionTFForm = new OptionTFQuestion();
                optionTFForm.OptionTrueFalseId = model.OptionTrueFalseId;
                if (optionTFForm.OptionTrueFalseId == 1) 
                {
                    OptionTFQuestion optionTFForm2 = new OptionTFQuestion();
                    optionTFForm2.QuestionId = model.QuestionId;
                    optionTFForm2.OptionTrueFalseId = 4;
                    _context.Add(optionTFForm2);
                    _context.SaveChanges();

                }
                else if (optionTFForm.OptionTrueFalseId == 3) 
                {
                    OptionTFQuestion optionTFForm2 = new OptionTFQuestion();
                    optionTFForm2.QuestionId = model.QuestionId;
                    optionTFForm2.OptionTrueFalseId = 2;
                    _context.Add(optionTFForm2);
                    _context.SaveChanges();
                }
                optionTFForm.QuestionId = model.QuestionId;

                
                _context.Add(optionTFForm);
                _context.SaveChanges();

                return RedirectToAction("EditQuestion", new { questionId = model.QuestionId }); ;
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult EditCourse(int courseId)
        {
            var course = _context.Courses.FirstOrDefault(c => c.Id == courseId);
            return View(course);
        }
        [HttpPost]
        public async Task<IActionResult> EditCourse(Course model)
        {
            
            if (ModelState.IsValid)
            {

                _context.Update(model);
                _context.SaveChanges();

                return RedirectToAction("Course", new { ID = model.Id });
            }
            return View(model);
        }
        
        [HttpGet]
        public IActionResult AddCourse()
        {

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddCourse(Course model)
        {
            var lecturer = _context.Lecturers.FirstOrDefault(n => n.UserName == User.Identity.Name);

            if (ModelState.IsValid)
            {
                _context.Add(model);
                _context.SaveChanges();

                LecturerCourse lc = new LecturerCourse();
                lc.LecturerId = lecturer.Id;
                lc.CourseId = (int)model.Id;
                _context.Add(lc);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }



        [HttpGet]
        public IActionResult GroupCourse(int courseId)
        {
            Console.WriteLine(courseId);
            var viewModel = new CourseViewModel
            {
                CourseId = courseId,
                Groups = _context.Groups.ToList()
            };
            return View(viewModel);
        }
        [HttpPost]
        public IActionResult GroupCourse(CourseViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                Console.WriteLine(viewModel.CourseId + "sdad ");
                viewModel.Groups = _context.Groups.ToList();
                return View(viewModel);
            }

            foreach (var groupId in viewModel.SelectedGroupIds)
            {
                GroupCourse gc = new GroupCourse
                {
                    CourseId = viewModel.CourseId,
                    GroupId = groupId 
                };
                if(_context.GroupCourse.Where(c => c.CourseId == gc.CourseId).Where(g => g.GroupId == gc.GroupId) == null)
                {
                    _context.GroupCourse.Add(gc);
                    _context.SaveChanges();
                }
            }

            return RedirectToAction("EditCourse", new {courseId  = viewModel.CourseId });
        }

        public async Task<IActionResult> Delete(int ID, int backId, int partId)
        {
            switch (partId)
            {
                case 1:
                    {
                        _context.Tests.Remove(_context.Tests.FirstOrDefault(i => i.Id == ID));
                        _context.SaveChanges();
                        return RedirectToAction("Course", new { ID = backId });

                    }
                case 2:
                    {
                        _context.Questions.Remove(_context.Questions.FirstOrDefault(i => i.Id == ID));
                        _context.SaveChanges();
                        return RedirectToAction("EditTest", new { testId = backId });
                    }
                case 3:
                    {
                        _context.Options.Remove(_context.Options.FirstOrDefault(i => i.Id == ID));
                        _context.SaveChanges();
                        return RedirectToAction("EditQuestion", new { questionId = backId });
                    }
                case 4:
                    {
                        var records = _context.OptionTFQuestion.Where(i => i.QuestionId == ID);
                        _context.OptionTFQuestion.RemoveRange(records);
                        _context.SaveChanges();
                        return RedirectToAction("EditQuestion", new { questionId = backId });
                    }
            }

            return View();
        }

    }
}
