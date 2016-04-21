using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Model
{
    public class AbscentDeduction
    {
        public int AbscentDeductionID { get; set; }
        [Display(Name = "Amount To Be Deducted")]
        [Range(0.0, 100000)]
        [Required]
        public decimal AmountToBeDeducted { get; set; }
    }
}
