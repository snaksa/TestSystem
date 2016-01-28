using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TestSystem.Data;
using TestSystem.Web.ViewModels;

namespace TestSystem.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var tests = DB.GetAllTests();
            return View(tests);
        }

        [HttpGet]
        public ActionResult Login()
        {
            if (Request.Cookies["userId"] != null)
            {
                return RedirectToAction("Index", "Admin");
            } 
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string username, string password)
        {
            User user = DB.GetUser(username, password);
            if (user == null) return RedirectToAction("Login");

            HttpCookie userId = new HttpCookie("userId", user.Id.ToString());
            userId.Expires = DateTime.Now.AddHours(5);
            Response.Cookies.Add(userId);

            return RedirectToAction("Index", "Admin");
        }

        [HttpGet]
        public ActionResult Logout()
        {
            string[] myCookies = Request.Cookies.AllKeys;
            foreach (string cookie in myCookies)
            {
                Response.Cookies[cookie].Expires = DateTime.Now.AddDays(-1);
            }

            return RedirectToAction("Index");
        }

        public ActionResult Test(int id)
        {
            var test = DB.GetTest(id);
            if (test == null) return RedirectToAction("Index");

            string[] q = new string[100];

            TestViewModel vm = new TestViewModel()
            {
                Test = test,
                Questions = q
            };

            return View("SingleTest", vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Test(TestViewModel vm)
        {
            if (vm.Name == null) vm.Name = "Без име";
            int studentId = DB.AddStudent(vm.Name, vm.Test.Id);

            var test = DB.GetTest(vm.Test.Id);
            int count = 0;
            List<StudentAnswer> answers = new List<StudentAnswer>();

            foreach (var item in test.TestQuestions)
            {
                StudentAnswer ans = new StudentAnswer()
                {
                    Answer = vm.Questions[count],
                    QuestionId = item.QuestionId,
                    StudentId = studentId,
                };
                answers.Add(ans);
                count++;
            }

            DB.AddAnswers(answers);

            TempData["success"] = "1";
            return RedirectToAction("Index");
        }
    }
}