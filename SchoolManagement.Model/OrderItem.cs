using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Model
{
   public class OrderItem
    {
       [Key]
     //  [DatabaseGenerated(DatabaseGeneratedOption.None)]
       public int OrderItemID { get; set; }
       public int OrderID { get; set; }
       public Int32 OrderNumber { get; set; }
       public string ItemName { get; set; }
       public int Quantity { get; set; }
       public decimal SubTotal { get; set; }
       public virtual Order Order { get; set; }
    }
}
