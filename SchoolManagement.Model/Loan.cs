using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Model
{
    public class Loan
    {
        public int LoanID { get; set; }


        public int StaffID { get; set; }

        public DateTime DateCreated { get; set; }
        public DateTime? DateApproved { get; set; }
        public string CreatedBy { get; set; }
        public string ApprovedBy { get; set; }

        [Required]
        [Display(Name = "Loan Amount")]
        [Range(0.0, Double.MaxValue)]
        public decimal LoanAmount { get; set; }

        public bool Approved { get; set; }

        [Display(Name = "First Month Payment")]
        public DateTime? FirstMonthPayment { get; set; }
        [Range(0.0, Double.MaxValue)]
        [Display(Name = "First Amount Payment")]
        public decimal? FirstAmountPayment { get; set; }

        [Display(Name = "Second Month Payment")]
        public DateTime? SecondMonthPayment { get; set; }
        [Range(0.0, Double.MaxValue)]
        [Display(Name = "Second Amount Payment")]
        public decimal? SecondAmountPayment { get; set; }

        [Display(Name = "Third Month Payment")]
        public DateTime? ThirdMonthPayment { get; set; }
        [Range(0.0, Double.MaxValue)]
        [Display(Name = "Third Amount Payment")]
        public decimal? ThirdAmountPayment { get; set; }

        [Display(Name = "Forth Month Payment")]
        public DateTime? ForthMonthPayment { get; set; }
        [Range(0.0, Double.MaxValue)]
        [Display(Name = "Forth Amount Payment")]
        public decimal? ForthAmountPayment { get; set; }

        [Display(Name = "Fifth Month Payment")]
        public DateTime? FifthMonthPayment { get; set; }
        [Range(0.0, Double.MaxValue)]
        [Display(Name = "Fifth Amount Payment")]
        public decimal? FifthAmountPayment { get; set; }

        [Display(Name = "Sixth Month Payment")]
        public DateTime? SixthMonthPayment { get; set; }
        [Range(0.0, Double.MaxValue)]
        [Display(Name = "Sixth Amount Payment")]
        public decimal? SixthAmountPayment { get; set; }

        [Display(Name = "Seventh Month Payment")]
        public DateTime? SeventhMonthPayment { get; set; }
        [Range(0.0, Double.MaxValue)]
        [Display(Name = "Seventh Amount Payment")]
        public decimal? SeventhAmountPayment { get; set; }

        //[Display(Name = "Eighth Month Payment")]
        //public DateTime? EighthMonthPayment { get; set; }
        //[Range(0.0, Double.MaxValue)]
        //[Display(Name = "Eight Amount Payment")]
        //public decimal? EightAmountPayment { get; set; }

        [Display(Name = "Ninth Month Payment")]
        public DateTime? NinthMonthPayment { get; set; }
        [Range(0.0, Double.MaxValue)]
        [Display(Name = "Ninth Amount Payment")]
        public decimal? NinthAmountPayment { get; set; }

        [Display(Name = "Tenth Month Payment")]
        public DateTime? TenthMonthPayment { get; set; }
        [Range(0.0, Double.MaxValue)]
        [Display(Name = "Tenth Amount Payment")]
        public decimal? TenthAmountPayment { get; set; }

        [Display(Name = "Eleventh Month Payment")]
        public DateTime? EleventhMonthPayment { get; set; }
        [Range(0.0, Double.MaxValue)]
        [Display(Name = "Eleventh Amount Payment")]
        public decimal? EleventhAmountPayment { get; set; }


         [Display(Name = "Twelfth Month Payment")]
        public DateTime? TwelfthMonthPayment { get; set; }
        [Range(0.0, Double.MaxValue)]
        [Display(Name = "Twelfth Amount Payment")]
        public decimal? TwelfthAmountPayment { get; set; }


    }
}
