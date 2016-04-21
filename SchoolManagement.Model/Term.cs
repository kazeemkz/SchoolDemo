using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Model
{
    public class Term
    {
        public int TermID { get; set; }
        [Required]
        [Display(Name = "Term Name")]
        public string TermName { get; set; }
        [Required]
        [Display(Name = "Term Starts")]
        public DateTime TermStarts { get; set; }
        [Required]
        [Display(Name = "Term Ends")]
        public DateTime TermEnds { get; set; }

        [Required]
        [Display(Name = "Session Start Year")]
        public String SessionStartYear { get; set; }
        [Required]
        [Display(Name = "Session End Year")]
        public String SessionEndYear { get; set; }


    }
}
