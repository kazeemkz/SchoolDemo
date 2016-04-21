using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Model
{
    public class Attendance
    {
        [Key]
       // [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int AttendanceID { get; set; }
        public string arm { get; set; }
        public string StudentID { get; set; }
        public DateTime DateTaken { get; set; }
        public bool Present { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string SurName { get; set; }
    }
}
