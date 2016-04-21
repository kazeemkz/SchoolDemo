using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Model
{
   public class Photo
    {
       [Key]
     //  [DatabaseGenerated(DatabaseGeneratedOption.None)]
       public int PhotoID { get; set; }
       public byte[] PhotoImage { get; set; }
       //public virtual PrimarySchoolStudent PrimarySchoolStudent { get; set; }
       public Person Person { get; set; }
      // public int UserID { get; set; }
       public int PersonID { get; set; }
    }
}
