//using AspNetCore;
using DistanceEducation.Data;
using System.Threading;
using DistanceEducation.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Security.Claims;
using DistanceEducation.Hubs;
using Microsoft.Data.SqlClient;
using System;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json.Linq;
using DistanceEducation.Models.Acount;

namespace DistanceEducation.Controllers
{
    public class TestController : Controller
    {
        private static ApplicationDbContext _context;
        private static IHubContext<TimerHub> _hub;
        public TestController(ApplicationDbContext context, IHubContext<TimerHub> hub)
        {
            _hub = hub;
            _context = context;
        }
      
        [HttpGet]
        public IActionResult TestInfo(int ID)
        {
            bool editor = false;
            var test = _context.Tests.FirstOrDefault(t => t.Id == ID);

            var questions = _context.Questions.Where(t => t.TestId == test.Id).ToList();
            double maxMark = 0;
            foreach(var question in questions)
            {
                maxMark += question.Cost;
            }

            ViewBag.maxMark = maxMark;

            var course = _context.Courses.FirstOrDefault(c => c.Id == test.CourseId);
            ViewBag.Course = course;

            if (User.IsInRole("Student")) 
            {
                var user = _context.Students.FirstOrDefault(n => n.UserName == User.Identity.Name);
                TestResult resultTest;
                try
                {
                    resultTest = _context.TestResult.Where(t => t.TestId == test.Id).FirstOrDefault(r => r.StudentId.Equals(user.Id));
                    if (resultTest != null)
                    {
                        if (resultTest.Mark != null)
                        {
                            ViewBag.Result = 0;
                            ViewBag.ResultMark = resultTest.Mark;
                        }
                        else
                        {
                            ViewBag.Result = 1;
                        }
                    }
                    else
                    {
                        ViewBag.Result = 2;
                    }

                }
                catch (Exception ex)
                {
                    ViewBag.Result = 2;
                }
            }
            else if (User.IsInRole("Lecturer"))
            {
                var user = _context.Lecturers.FirstOrDefault(n => n.UserName == User.Identity.Name);
                var editors = _context.LecturerCourse.Where(c => c.CourseId == course.Id).ToList();
                foreach (var i in editors)
                {
                    if (i.LecturerId == user.Id)
                    {
                        var resultTestCount = _context.TestResult.Where(t => t.TestId == ID).Where(r => r.Mark == null);
                        ViewBag.Count = resultTestCount.Count();
                        var resultTest = _context.TestResult.Where(t => t.TestId == ID).Where(r => r.Mark != null).Include(s => s.Student).Include(g => g.Student.Group);
                        ViewBag.Results = resultTest;
                        editor = true;
                        break;
                    }
                }
                ViewBag.Result = 2;
            }

            ViewBag.isEditor = editor;
            return View(test);
        }
        [HttpGet] 
        public IActionResult Test(int ID)
        {
            var test = _context.Tests.FirstOrDefault(t => t.Id == ID);
            ViewBag.Test = test;
            var questions = _context.Questions.Where(q => q.TestId == ID).ToList();
            ViewBag.Questions = questions;
            var options = _context.Options.Where(a => a.Question.TestId == ID).ToList();
            ViewBag.Options = options;
            var optionsTF = _context.OptionTFQuestion.Where(b => b.Question.TestId == ID).Include(p => p.OptionTrueFalse).ToList();
            ViewBag.OptionsTF = optionsTF;
            Console.WriteLine(optionsTF.Count());

            return View();
        }
        [HttpPost]
        public IActionResult SaveAnswers(IFormCollection form, int testId)
        {
            Console.WriteLine("Test-" + testId);

            var UserName = User.Identity.Name;
            var user = _context.Students.FirstOrDefault(n => n.UserName == UserName);

            var questions = _context.Questions.Where(q => q.TestId == testId).ToList();
            TestResult testResult = new TestResult();
            testResult.StudentId = user.Id;
            testResult.TestId = testId;

            _context.TestResult.Add(testResult);
            _context.SaveChanges();

            var testResultId = testResult.Id;

            foreach (var question in questions)
            {
                Answer answer = new Answer();
                answer.QuestionId = question.Id;

                foreach (var key in form.Keys)
                {
                    if (question.QuestionTypeId == 1 && key.Contains("free-question-"))
                    {
                        Console.WriteLine(1 + key + ":" + form[key]);

                        answer.TextAnswer = form[key];
                        var option = _context.Options.FirstOrDefault(q => q.QuestionId == question.Id);
                        if (option.Text.Contains(form[key]))
                        {
                            answer.Correct = true;
                        }
                        else
                        {
                            answer.Correct = false;
                        }
                        answer.TestResultId = testResultId;
                        _context.Answers.Add(answer);
                        _context.SaveChanges();
                    }
                    else if (question.QuestionTypeId == 2 && key.Contains("single-answer-"))
                    {
                        Console.WriteLine(2 + key + ":" + form[key]);
                        int.TryParse(form[key], out int intValue);
                        answer.OptionId = intValue;
                        var option = _context.Options.FirstOrDefault(q => q.Id == intValue);
                        answer.Correct = (bool)option.Correct;
                        answer.TestResultId = testResultId;
                        _context.Answers.Add(answer);
                        _context.SaveChanges();
                    }
                    else if (question.QuestionTypeId == 3 && key.Contains("multiple-answer-"))
                    {
                        Console.WriteLine(3 + key + ":" + form[key]);

                        foreach (var ans in form[key]) 
                        { 
                            Answer answerList = new Answer();
                            answerList = answer;
                            answerList.Id = null;

                            int.TryParse(ans, out int intValue);
                            answerList.OptionId = intValue;
                            var option = _context.Options.FirstOrDefault(q => q.Id == intValue);
                            answerList.Correct = (bool)option.Correct;
                            Console.WriteLine("Znach" + intValue);
                            Console.WriteLine("QuestionId" + answerList.QuestionId);
                            Console.WriteLine("OptionId" + answerList.OptionId);
                            Console.WriteLine("Correct" + answerList.Correct);
                            answerList.TestResultId = testResultId;
                            _context.Answers.Add(answerList);
                            _context.SaveChanges();
                        }
                        
                    }
                    else if (question.QuestionTypeId == 4 && key.Contains("true-false-answer-"))
                    {
                        Console.WriteLine(4 + key + ":" + form[key]);
                        int.TryParse(form[key], out int intValue);
                        answer.OptionTrueFalseId = intValue;
                        var option = _context.OptionTrueFalse.FirstOrDefault(q => q.Id == intValue);
                        answer.Correct = (bool)option.Correct;
                        answer.TestResultId = testResultId;
                        _context.Answers.Add(answer);
                        _context.SaveChanges();
                    }
                }            
            }
            return RedirectToAction("Index", "Home");
        }

        public IActionResult RateTheWorks(int Id)
        {
            var notRate = _context.TestResult.Where(r => r.Mark == null).Where(t => t.TestId == Id).ToList();
            foreach (var item in notRate) 
            {
                float cost = 0;
                int countOptions = 0;
                float costOne = 0;
                int multipleId = 0;

                float studentMark = 0;

                var answers = _context.Answers.Where(a => a.TestResultId == item.Id).Include(q => q.Question).ToList();
                foreach (var answer in answers) 
                {
                    switch (answer.Question.QuestionTypeId)
                    {
                        case 1: 
                            {
                                if (answer.Correct)
                                {
                                    studentMark += answer.Question.Cost;
                                }
                                break; 
                            }
                        case 2: 
                            {
                                if (answer.Correct)
                                {
                                    studentMark += answer.Question.Cost;
                                }
                                break;
                            }
                        case 3: 
                            {
                                if (multipleId != answer.QuestionId)
                                {
                                    multipleId = answer.QuestionId;
                                    cost = answer.Question.Cost;
                                    countOptions = _context.Options.Where(t => t.QuestionId == answer.QuestionId).Where(c => c.Correct == true).Count();
                                    costOne = cost / countOptions;
                                }

                                if (answer.Correct)
                                {
                                    studentMark += costOne;
                                }
                                break; 
                            }
                        case 4: 
                            {
                                if (answer.Correct)
                                {
                                    studentMark += answer.Question.Cost;
                                }
                                break; 
                            }

                    }
                }
                double roundedStudentMark = Math.Round(studentMark, 2);
                item.Mark = roundedStudentMark;
                _context.Update(item);
                _context.SaveChanges();
            }

            return RedirectToAction("TestInfo", new { ID = Id });
        }























        [HttpGet]
        public IActionResult GetResultsByGroup(int groupid)
        {
            var resultTest = _context.TestResult.Where(r => r.Mark != null);

            if (groupid != 0)
            {
                resultTest = resultTest.Include(s => s.Student).Include(g => g.Student.Group).Where(g => g.Student.GroupId == groupid);
                ViewBag.Results = resultTest;
            }
            else
            {
                resultTest = resultTest.Include(s => s.Student).Include(g => g.Student.Group);
            }
            return PartialView("ListStudents", resultTest);
        }
    }
}