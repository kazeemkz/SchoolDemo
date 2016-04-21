using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Model
{
    public class SchoolFeesAccount
    {
        public int SchoolFeesAccountID { get; set; }
        [Required]
        public decimal Amount { get; set; }
        public string TransactionType { get; set; } // credit or debit
        public string TransactionMethod { get; set; } // for cash deposit


        //  [Display(Name = "Teller Number")]
        public int StudentID { get; set; }

        [Display(Name = "Teller Number")]
        public string TellerNumber { get; set; }

        [Display(Name = "Cheque Number")]
        public string ChequeNumber { get; set; }

        [Display(Name = "Bank Draft Number")]
        public string BankDraftNumber { get; set; }

        [Display(Name = "POS Transaction Number")]
        public string POSTransactionNumber { get; set; }

        public DateTime PaidDate { get; set; }
        public DateTime DateApproved { get; set; }
        public string EnteredBy { get; set; }
        public string ApprovedBy { get; set; }
        public string Session { get; set; }
        public string Level { get; set; }


    }
}
