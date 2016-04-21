using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Model
{
    public class Course
    {
        public int CourseID { get; set; }

       // rtgegrtt
       // [Required(ErrorMessage="Level is Required")]
        [NotMapped]
        [Required(ErrorMessage = "Level is Required")]
        public string LevelStringID {get; set;}
        public int LevelID { get; set; }
       // public virtual Level Level { get; set; }
        [Required]
       // [RegularExpression(@"(\S)+", ErrorMessage = "White Space is Not Allowed, Use - to Seperate Words")]
        public string Name { get; set; }
        public string path { get; set; }
     //   public ICollection<Chapter> Chapter { get; set; }
      //  public ICollection<TextBook> TextBook { get; set; }
      //  public ICollection<Exam> Exam { get; set; }



    }
}
