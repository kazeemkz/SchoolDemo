using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Model
{
  public  class PractiseSave
    {
      [Key]
     // [DatabaseGenerated(DatabaseGeneratedOption.None)]
      public int PractiseSaveID { get; set; }
      public string StudentID { get; set; }
      public string Level { get; set; }
      public string LevelType { get; set; }
      public string Term { get; set; }
      public string Subject { get; set; }
      public string Code { get; set; }
      public decimal Score { get; set; }
    }
}
