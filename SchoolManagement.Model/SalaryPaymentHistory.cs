using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Model
{
   public class SalaryPaymentHistory
    {
       public int SalaryPaymentHistoryID { get; set; }

        public string Description { get; set; }

        public DateTime DatePaid { get; set; }

        public int StaffID { get; set; }

        public string LastName { get; set; }
        public string FirstName { get; set; }

        public decimal AmountPaid { get; set; }

        public decimal ActualSalary { get; set; }

        public bool PaySalary { get; set; }

       [NotMapped]
        public List<Deduction> TheDeduction { get; set; }

       [NotMapped]
       public LatenessDeduction TheLatenessDeduction { get; set; }

       [NotMapped]
       public List<Loan> TheLoan { get; set; }

       [NotMapped]
       public decimal? TotalLoan { get; set; }

       [NotMapped]
       public decimal? TotalLatenessDeduction { get; set; }

       [NotMapped]
       public decimal? TotalAbscentDeduction { get; set; }
    }
}
