//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TestSystem.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class StudentAnswer
    {
        public int Id { get; set; }
        public int QuestionId { get; set; }
        public string Answer { get; set; }
        public int StudentId { get; set; }
    
        public virtual Question Question { get; set; }
        public virtual Student Student { get; set; }
    }
}