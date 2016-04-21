using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Model
{
  public  class TextBook
    {
      public int TextBookID {get;set;}
      public string Name { get; set; }
      public string path { get; set; }

      public string ParentName { get; set; }
      public int CourseID { get; set; }
      public virtual Course Course { get; set; }
      public byte[] FileData { get; set; }
    }
}
