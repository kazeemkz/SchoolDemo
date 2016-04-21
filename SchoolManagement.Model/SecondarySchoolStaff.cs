using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Model
{
   public class SecondarySchoolStaff
    {
      // [Key]
      // [DatabaseGenerated(DatabaseGeneratedOption.None)]
       public int SecondarySchoolStaffID { get; set; }
        public string FirstName { get; set; }
        public string LasttName { get; set; }
        public DateTime EnrollmentDate { get; set; }
        public Level PresentLevel { get; set; }
        public Level InitialLevel { get; set; }
    }
}
