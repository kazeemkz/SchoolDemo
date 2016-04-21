using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Model
{
  public  class AuditTrail
    {
      public int AuditTrailID { get; set; }
      public string UserID { get; set; }
      public DateTime Date { get; set; }
      public string Action { get; set; }
    }
}
