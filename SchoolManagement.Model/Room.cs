using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Model
{
   public class Room
    {
       public int RoomID { get; set; }
       [Display(Name="Room Name")]
       [Required]
       public string RoomName { get; set; }
       public virtual Hostel Hostel { get; set; }
       public int HostelID { get; set; }
       [Display(Name="How many Students can room contain?")]
       [Required]
       public int Capacity { get; set; }

       //public int Capacitytwo { get; set; }
    }
}
