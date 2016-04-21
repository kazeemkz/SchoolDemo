using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Model
{
    public class Parent// : Person
    {
        public int ParentID { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string MiddleName { get; set; }
        [Required]
        public string LastName { get; set; }
        public DateTime Date { get; set; }
        public int UserID { get; set; }
        public string Role { get; set; }
        [NotMapped]
        [RegularExpression("^([a-zA-Z0-9 .&'-]+)$", ErrorMessage = "Invalid")]
        public string StudentIDOne { get; set; }
        [NotMapped]
        [RegularExpression("^([a-zA-Z0-9 .&'-]+)$", ErrorMessage = "Invalid")]
        public string StudentIDTwo { get; set; }
        [NotMapped]
        [RegularExpression("^([a-zA-Z0-9 .&'-]+)$", ErrorMessage = "Invalid")]
        public string StudentIDThree { get; set; }
        [NotMapped]
        [RegularExpression("^([a-zA-Z0-9 .&'-]+)$", ErrorMessage = "Invalid")]
        public string StudentIDFour { get; set; }
        [NotMapped]
        [RegularExpression("^([a-zA-Z0-9 .&'-]+)$", ErrorMessage = "Invalid")]
        public string StudentIDFive { get; set; }

        public ICollection<PrimarySchoolStudent> ThePrimarySchoolStudent { get; set; }
    }
}
