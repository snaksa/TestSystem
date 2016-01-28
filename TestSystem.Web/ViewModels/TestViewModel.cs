using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TestSystem.Data;

namespace TestSystem.Web.ViewModels
{
    public class TestViewModel
    {
        public string Name { get; set; }
        public Test Test { get; set; }
        public string[] Questions { get; set; }
    }
}