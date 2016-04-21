using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Model
{
  public  class DeductionHistory
    {
      public int DeductionHistoryID { get; set; }

      public string Description { get; set; }

      public DateTime DatePaid { get; set; }

      public string StaffID { get; set; }

      public decimal AmountDeducted { get; set; }
    }
}
