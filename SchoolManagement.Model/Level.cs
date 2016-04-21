using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Model
{
  public class Level
    {
      [Key]
     // [DatabaseGenerated(DatabaseGeneratedOption.None)]
      public int LevelID { get; set; }
      [Display(Name="Class Name")]
      [Required]
      public string LevelName { get; set; }
        [Required]
      [Display(Name = "Class Arm")]
      public string Type { get; set; }


    }
}
