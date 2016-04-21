using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SchoolManagement.Model
{
    public class Grade
    {
        public double TotalPoints { get; set; }
        public double Score { get; set; }
        public OnlineExam Exam { get; set; }
    }
}
