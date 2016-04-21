using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Model
{
   public class StudentFees
    {
       [Key]
       //[DatabaseGenerated(DatabaseGeneratedOption.None)]
       public int StudentFeesID { get; set; }
     //  [Required]
       public int SchoolFeesTypeID { get; set; }
       // [Display(Name = "Item Name")]
       [Required]
       public string ItemName { get; set; }
       [Required]
       public string Term { get; set; }
      // public int StudentID { get; set; } 
       [Required]
       public decimal Cost { get; set; }
    }
}
