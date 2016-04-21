using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SchoolManagement.Model;

namespace SchoolManagement.Models
{
    public class SubRegistrationViewModel
    {
        public SubjectRegistration SubjectRegistration { get; set; }
        public string Level { get; set; }
        public IList<Subject> Subjects { get; set; }
    }
}