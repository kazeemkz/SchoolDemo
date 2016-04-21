using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Model
{
    public class AdditionalChapterText
    {
        public int AdditionalChapterTextID { get; set; }
        public int ChapterID { get; set; }
        public string Name { get; set; }
        public virtual Chapter Chapter { get; set; }
        public string Path { get; set; }
        public string ParentName { get; set; }
        public byte[] FileData { get; set; }
    }
}
