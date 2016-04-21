using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Model
{
    public class Chapter
    {
        public int ChapterID { get; set; }
        public ICollection<AdditionalChapterText> AdditionalChapterText { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public int CourseID { get; set; }
        public string Number { get; set; }
        public virtual Course Course { get; set; }
        public string ParentName { get; set; }
      public byte[] FileData { get; set; }
      //  public string 

    }
}
