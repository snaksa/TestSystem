using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TestSystem.Data;
using TestSystem.Web.Filters;
using TestSystem.Web.ViewModels;

namespace TestSystem.Web.Controllers
{
    [Logged]
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            var tests = DB.GetAllTests();
            if (tests == null) tests = new List<Test>();
            return View(tests);
        }

        [HttpGet]
        public ActionResult EditTest(int id)
        {
            var test = DB.GetTest(id);
            if (test == null) return RedirectToAction("Index");

            EditTestViewModel vm = new EditTestViewModel()
            {
                Test = test,
                Questions = new Question[100]
            };
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditTest(EditTestViewModel vm)
        {
            DB.UpdateTest(vm.Test);
            DB.UpdateTestQuestions(vm.Questions);
            return RedirectToAction("EditTest", new { id = vm.Test.Id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddQuestion(EditTestViewModel vm)
        {
            Question q = new Question();
            q.Title = vm.AddQuestion.Title;
            q.QuestionType = vm.AddQuestion.QuestionType;
            if (vm.AddQuestion.QuestionType == 2)
            {
                q.CorrectAnswer = vm.AddQuestion.CorrectAnswer;
            }
            else if (vm.AddQuestion.QuestionType == 4)
            {
                q.Answer1 = vm.AddQuestion.Answer1;
                q.Answer2 = vm.AddQuestion.Answer2;
                q.Answer3 = vm.AddQuestion.Answer3;
                q.Answer4 = vm.AddQuestion.Answer4;
                q.CorrectAnswer = vm.AddQuestion.CorrectAnswer;
            }

            DB.AddQuestion(vm.Test.Id, q);

            return RedirectToAction("EditTest", new { id = vm.Test.Id });
        }

        [HttpGet]
        public ActionResult DeleteQuestion(int id, int test)
        {
            DB.DeleteQuestion(id);
            return RedirectToAction("EditTest", new { id = test });
        }

        [HttpGet]
        public ActionResult Answers(int id, int test)
        {
            var answers = DB.GetStudentAnswers(id);
            var t = DB.GetTest(test);
            if (answers.Count == 0) return RedirectToAction("Index");
            string name = answers[0].Student.Name;
            string title = t.Title;
            AnswersViewModel vm = new AnswersViewModel()
            {
                Answers = answers,
                Name = name,
                Title = title
            };

            return View("Answers", vm);
        }

        [HttpGet]
        public ActionResult TestAnswers(int id)
        {
            var students = DB.GetStudents(id);
            return View(students);
        }

        [HttpGet]
        public ActionResult DeleteTest(int id)
        {
            DB.DeleteTest(id);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult NewTest()
        {
            string title = "Нов тест";
            int t = DB.AddNewTest(title);
            return RedirectToAction("EditTest", new { id = t });
        }
    }
}