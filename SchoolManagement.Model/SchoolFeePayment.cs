using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Model
{
    public class SchoolFeePayment
    {

        [Key]

        public int SchoolFeePaymentID { get; set; }
        [Display(Name = "School Fees Type")]
        public string TheSchoolFeesType { get; set; }
        //  public string Level { get; set; }
        public string Level { get; set; }
        //  [Display(Name = "Item Name")]
        //  public string ItemName { get; set; }
        public int StudentID { get; set; }
        public decimal Cost { get; set; }
        public decimal Owing { get; set; }

        [Display(Name = "Date Paid")]
        public DateTime DatePaid { get; set; }
        [Required]
        public decimal Paid { get; set; }
        public string Term { get; set; }
        public string EnteredBy { get; set; }

        public string Session { get; set; }
        // [Required]
        // [Display(Name = "Payment Method")]
        //  public int PaymentMethod { get; set; }

        public string PaymentMethod { get; set; }

        [Display(Name = "Teller Number")]
        public string TellerNumber { get; set; }

        [Display(Name = "Cheque Number")]
        public string ChequeNumber { get; set; }

        [Display(Name = "Bank Draft Number")]
        public string BankDraftNumber { get; set; }

        [Display(Name = "POS Transaction Number")]
        public string POSTransactionNumber { get; set; }

        public bool Approved { get; set; }
    }
}
