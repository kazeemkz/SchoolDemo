using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Model
{
    public class TermRegistration
    {
        public int TermRegistrationID { get; set; }

        public int StudentID { get; set; }
        public string Level { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
       // [Required]
        public string Session { get; set; }
       // [Required]
        public string Term { get; set; }
        public bool Register { get; set; }
       // [Required]
        public string SchoolFeesKind { get; set; }
        public string Sex { get; set; }
        public DateTime DateRegistered { get; set; }

        public decimal Cost { get; set; }
        [NotMapped]
        public decimal Owing { get; set; }
    }
}
