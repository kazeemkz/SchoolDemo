using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Model
{
  public  class Order
    {
      [Key]
      //[DatabaseGenerated(DatabaseGeneratedOption.None)]
      public int OrderID { get; set; }
      public string Term { get; set; }
      public Int32 studentID { get; set; }
      public Int32 OrderNumber { get; set; }
     [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd/MM/yyyy  HH:mm:ss}", ApplyFormatInEditMode = true)]
      public DateTime? OrderDate { get; set; }
      public string StudentLastName { get; set; }
      public int ItemQuantity { get; set; }
      public decimal TotalAmount { get; set; }
      public string Level { get; set; }
      public bool Approved { get; set; }
      List<OrderItem> theOrderItems { get; set; }
    }
}
