using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace TestSystem.Data
{
    public static class DB
    {
        public static List<Test> GetAllTests()
        {
            var db = new TestSystemEntities();
            var tests = db.Tests.ToList();
            db.Dispose();
            return tests;
        }

        public static Test GetTest(int id)
        {
            var db = new TestSystemEntities();
            var test = db.Tests
                .Include(p => p.TestQuestions.Select(a => a.Question))
                .Where(t => t.Id == id)
                .FirstOrDefault();
            return test;
        }

        public static void UpdateTest(Test test)
        {
            using (var db = new TestSystemEntities())
            {
                db.Tests.Add(test);
                var entry = db.Entry(test);
                entry.State = EntityState.Modified;
                db.SaveChanges();
            }
        }

        public static void UpdateTestQuestions(Question[] questions)
        {
            if (questions == null) return;
            using (var db = new TestSystemEntities())
            {
                foreach (var item in questions)
                {
                    db.Questions.Add(item);
                    var entry = db.Entry(item);
                    entry.State = EntityState.Modified;
                }
                db.SaveChanges();
            }
        }

        public static void AddQuestion(int test, Question q)
        {
            using (var db = new TestSystemEntities())
            {
                db.Questions.Add(q);
                var entry = db.Entry(q);
                entry.State = EntityState.Added;
                db.SaveChanges();

                TestQuestion t = new TestQuestion()
                {
                    QuestionId = q.Id,
                    TestId = test
                };
                db.TestQuestions.Add(t);
                db.SaveChanges();
            }
        }

        public static void DeleteQuestion(int id)
        {
            using (var db = new TestSystemEntities())
            {
                db.StudentAnswers.RemoveRange(db.StudentAnswers.Where(s => s.QuestionId == id));
                db.TestQuestions.Remove(db.TestQuestions.Where(q => q.QuestionId == id).FirstOrDefault());
                db.Questions.Remove(db.Questions.Where(q => q.Id == id).FirstOrDefault());
                db.SaveChanges();
            }
        }

        public static int AddStudent(string p, int test)
        {
            var db = new TestSystemEntities();
            Student stud = new Student(){
                Name = p,
                TestId = test
            };
            db.Students.Add(stud);
            db.SaveChanges();
            db.Dispose();

            return stud.Id;
        }

        public static void AddAnswers(List<StudentAnswer> answers)
        {
            using (var db = new TestSystemEntities())
            {
                foreach (var item in answers)
                {
                    db.StudentAnswers.Add(item);
                }
                db.SaveChanges();
            }
        }

        public static List<StudentAnswer> GetStudentAnswers(int id)
        {
            var db = new TestSystemEntities();
            var answers = db.StudentAnswers
                .Include(m => m.Question)
                .Include(q => q.Student)
                .Where(s => s.StudentId == id)
                .OrderBy(o => o.QuestionId)
                .ToList();
            db.Dispose();
            return answers;
        }

        public static List<Student> GetStudents(int id)
        {
            var db = new TestSystemEntities();
            var students = db.Students.Where(s => s.TestId == id).ToList();
            db.Dispose();
            return students;
        }

        public static void DeleteTest(int id)
        {
            var db = new TestSystemEntities();
            var test = db.Tests.Where(t => t.Id == id).FirstOrDefault();;
            var students = db.Students.Where(s => s.TestId == id).ToList();

            foreach (var stud in students)
            {
                db.StudentAnswers.RemoveRange(db.StudentAnswers.Where(s => s.StudentId == stud.Id));
                db.Students.Remove(db.Students.Where(s => s.Id == stud.Id).FirstOrDefault());
            }

            var questions = db.TestQuestions.Where(t => t.TestId == test.Id).Select(s => s.Question).ToList();
            db.TestQuestions.RemoveRange(db.TestQuestions.Where(t => t.TestId == test.Id));
            db.Questions.RemoveRange(questions);
            db.Tests.Remove(test);

            db.SaveChanges();
            db.Dispose();
        }

        public static int AddNewTest(string title)
        {
            Test t = new Test()
            {
                Title = title
            };
            using (var db = new TestSystemEntities())
            {
                
                db.Tests.Add(t);
                db.SaveChanges();
            }
            return t.Id;
        }

        public static User GetUser(string username, string password)
        {
            var db = new TestSystemEntities();
            var user = db.Users.Where(u => u.Username == username && u.Password == password).FirstOrDefault();
            db.Dispose();
            return user;
        }
    }
}
