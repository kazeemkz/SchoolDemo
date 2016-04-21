using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Model
{
    public class AttendanceStaff
    {
        [Key]
        // [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int AttendanceStaffID { get; set; }
        //  public string arm { get; set; }
        public string StaffID { get; set; }
      //  [Required]
        public DateTime DateTaken { get; set; }
        public bool Present { get; set; }
        [Display(Name = "Not Present But Took Permission")]
        public bool NotPresentButTookPermission { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string SurName { get; set; }
    }
}
