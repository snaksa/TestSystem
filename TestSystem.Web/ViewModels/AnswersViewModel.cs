using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TestSystem.Data;

namespace TestSystem.Web.ViewModels
{
    public class AnswersViewModel
    {
        public string Name { get; set; }
        public string Title { get; set; }
        public List<StudentAnswer> Answers { get; set; }
    }
}