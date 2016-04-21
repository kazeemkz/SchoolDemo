using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Model
{
    public class Salary
    {
        public int SalaryID { get; set; }
        [Required]
        public decimal Amount { get; set; }

        [Display(Name = "Salary Description")]
        [Required]
        public string SalaryDescription { get; set; }
    }
}
