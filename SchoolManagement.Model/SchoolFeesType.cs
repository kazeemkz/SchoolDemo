using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Model
{
    //using System.ComponentModel.DataAnnotations;
    public class SchoolFeesType
    {
        [Key]
        public int SchoolFeesTypeID { get; set; }
        [Display(Name = " School Fees Type")]
        [Required]
        public string SchoolFeesKind { get; set; }
        [Required]
        public decimal Amount { get; set; }

        // public int Kanta { get; set; }

    }
}