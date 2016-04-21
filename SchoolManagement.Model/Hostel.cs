using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Model
{
  public  class Hostel
    {
      public int HostelID { get; set; }
      [Required]
     [Display(Name= "Hostel Name")]
      public string HostelName {get;set;}
      [Display(Name= "Room Number(s)")]
      public int RoomNumber { get; set; }
      public ICollection<Room> theRooms { get; set; }

    }
}
