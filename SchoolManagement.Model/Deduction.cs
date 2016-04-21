using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Model
{
    public class Deduction
    {
        public int DeductionID { get; set; }
        [Required]
        public decimal Amount { get; set; }
        [NotMapped]
        public bool Selected { get; set; }

        [Display(Name = "Deduction Description")]
        [Required]
        public string DeductionDescription { get; set; }

       // public virtual PrimarySchoolStaff PrimarySchoolStaff { get; set; }
       // public int PersonID { get; set; }
      //  public ICollection<PrimarySchoolStaff> ThePrimarySchoolStaff { get; set; }
    }
}
