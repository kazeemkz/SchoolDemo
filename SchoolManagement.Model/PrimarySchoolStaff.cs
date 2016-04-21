using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Model
{
    public class PrimarySchoolStaff : Person
    {

        [RegularExpression(@"^[0-9]{0,15}$", ErrorMessage = "Telephone Number should contain only numbers")]
        public string Telephone { get; set; }

        [Display(Name = "Subject Strength")]
        public string SubjectStrength { get; set; }


        [Display(Name = "Class Teacher Of")]
        public string ClassTeacherOf { get; set; }
        //public string Role { get; set; }

        [Display(Name = "Email Address")]
        [RegularExpression(@"^\w+@[a-zA-Z_]+?\.[a-zA-Z]{2,3}$", ErrorMessage = "Email is not in proper format")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }


        [Display(Name = "Hightest Qualification")]
        public string HightestQualification { get; set; }

        //account details
        [Display(Name = "Account Name")]
        [Required]
        public string AccountName { get; set; }

        [Display(Name = "Account Number")]
        [Required]
        public string AccountNumber { get; set; }

        [Display(Name = "Bank Name")]
        [Required]
        public string BankName { get; set; }

        [Display(Name = "Bank Address")]
        // [Required]
        public string BankAddress { get; set; }

      //  [Required]
        public int? SalaryID { get; set; }

        public bool? LatenessDeduction { get; set; }

        public bool? AbscentDeduction { get; set; }
     //   public string Deduction { get; set; }

        public ICollection<Deduction> TheDeduction { get; set; }

        public string ContributionIDs { get; set; }
        //public bool IsApproved { get; set; }

        //public string CreatedUserName { get; set; }
        //public string ApprovedUserName { get; set; }
        //public string DeletedUserName { get; set; }
        //public DateTime DateEntered { get; set; }
        //public DateTime DateApproved { get; set; }

    }
}
