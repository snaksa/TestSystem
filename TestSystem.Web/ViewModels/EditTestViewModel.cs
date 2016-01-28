using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TestSystem.Data;

namespace TestSystem.Web.ViewModels
{
    public class EditTestViewModel
    {
        public Test Test { get; set; }
        public Question[] Questions { get; set; }
        public Question AddQuestion { get; set; }
    }
}