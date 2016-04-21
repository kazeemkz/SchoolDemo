using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Model
{
    public class Exam2 : ExamBoss
    {
        [Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Exam2ID { get; set; }
        public string Term { get; set; }
        public string Level { get; set; }
        public string SubjectName { get; set; }
       // [Range(0.0, 20.0)]
        public decimal FirstCA { get; set; }
       // [Range(0.0, 60.0)]
        public decimal SubjectExam { get; set; }
        //[Range(0.0, 20.0)]
        public decimal SecondCA { get; set; }
        public int StudentCode { get; set; }
        public string Session { get; set; }
        //  public decimal Total { get; set; }
    }
}
