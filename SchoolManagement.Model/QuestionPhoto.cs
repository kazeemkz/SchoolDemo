using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Model
{
  public  class QuestionPhoto
    {
      [Key]
      //[DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int QuestionPhotoID { get; set; }

        public virtual Question Question { get; set; }
        public virtual OnlineExam OnlineExam { get; set; }
        public int QuestionID { get; set; }
        public int OnlineExamID { get; set; }
        public byte[] FileData { get; set; }
    }
}
