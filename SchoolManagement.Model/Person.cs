using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Model
{
    public class Person
    {
        [Key]
        // [DatabaseGenerated(DatabaseGeneratedOption.None)]
        //  [Key]
        // [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int PersonID { get; set; }
        [Display(Name = "First Name")]
        [Required]
        public string FirstName { get; set; }

        public int UserID { get; set; }
        [Display(Name = "Middle Name")]
        [Required]
        public string Middle { get; set; }
        [Display(Name = "Last Name")]
        [Required]
        public string LastName { get; set; }
        [Required, DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd/MM/yyyy  HH:mm:ss}", ApplyFormatInEditMode = true)]
        [Display(Name = "Enrollment Date")]
        //[Required, DataType(DataType.Date)]
        public DateTime EnrollmentDate { get; set; }
        [Required]
        public string Sex { get; set; }


        //  [Required, DataType(DataType.Date)]
        // [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd/MM/yyyy }", ApplyFormatInEditMode = true)]
        [Display(Name = "Date of Birth")]
        public DateTime DOB { get; set; }


        [Display(Name = "Blood Group")]
        public string BloodGroup { get; set; }

        public string Genotype { get; set; }
        [Display(Name = "State Disability if Any")]
        public string Disability { get; set; }

        public bool IsApproved { get; set; }
        public string Role { get; set; }

        //public string CreatedUserName { get; set; }
        public string ApprovedUserName { get; set; }
        // public string DeletedUserName { get; set; }
        // public DateTime DateEntered { get; set; }

        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd/MM/yyyy  HH:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime? DateApproved { get; set; }
        //[DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd/MM/yyyy }", ApplyFormatInEditMode = true)]
        [Display(Name = "Place Of Birth")]
        public string PlaceOfBirth { get; set; }
        public int SchoolFeesTypeID { get; set; }
    }
}
