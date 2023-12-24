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
            var test = _context.Tests.FirstOrDefault(t => t.Id == ID);

            var course = _context.Courses.FirstOrDefault(c => c.Id == test.CourseId);
            ViewBag.Course = course;

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
            var questions = _context.Questions.Where(q => q.TestId == testId).ToList();
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
                    }
                    else if (question.QuestionTypeId == 2 && key.Contains("single-answer-"))
                    {
                        Console.WriteLine(2 + key + ":" + form[key]);
                        int.TryParse(form[key], out int intValue);
                        answer.OptionId = intValue;
                        var option = _context.Options.FirstOrDefault(q => q.Id == intValue);
                        answer.Correct = option.Correct;
                    }
                    else if (question.QuestionTypeId == 3 && key.Contains("multiple-answer-"))
                    {
                        Console.WriteLine(3 + key + ":" + form[key]);

                        foreach (var ans in form[key]) 
                        { 
                            Answer answerList = new Answer();
                            answerList = answer;
                            int.TryParse(ans, out int intValue);
                            answerList.OptionId = intValue;
                            var option = _context.Options.FirstOrDefault(q => q.Id == intValue);
                            answerList.Correct = option.Correct;
                            Console.WriteLine("Znach" + intValue);
                            Console.WriteLine("QuestionId" + answerList.QuestionId);
                            Console.WriteLine("OptionId" + answerList.OptionId);
                            Console.WriteLine("Correct" + answerList.Correct);
                        }
                        
                    }
                    else if (question.QuestionTypeId == 4 && key.Contains("true-false-answer-"))
                    {
                        Console.WriteLine(4 + key + ":" + form[key]);
                        int.TryParse(form[key], out int intValue);
                        answer.OptionTFId = intValue;
                        var option = _context.OptionTrueFalse.FirstOrDefault(q => q.Id == intValue);
                        answer.Correct = option.Correct;
                    }
                }
                if (question.QuestionTypeId != 3) 
                {
                    Console.WriteLine("QuestionId " + answer.QuestionId);
                    Console.WriteLine("OptionId " + answer.OptionId); 
                    Console.WriteLine("OptionTFId" + answer.OptionTFId);
                    Console.WriteLine("TextAnswer" + answer.TextAnswer);
                    Console.WriteLine("Correct" + answer.Correct);
                }               
            }
            return RedirectToAction("Index", "Home");
        }
    }
}