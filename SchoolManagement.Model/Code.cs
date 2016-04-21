using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Model
{
   public class Code
    {
      [Key]
       public int CodeID { get; set; }

       public string Value { get; set; }

       public DateTime DateCreated {get; set;}

       public bool GivenOut {get; set;}
     
    }
}

 //public class Subject
 //   {
 //       public int SubjectID { get; set; }
 //       //  public string IsSubjectNameExists(string Name)
 //      //[Remote("IsSubjectNameExists", "Subject", ErrorMessage="Can't Add What Already Exist")]
 //      // [Required]
 //       public string Name { get; set; }
 //       [Range(typeof(decimal),"0.0","60.0")]
 //       public decimal? SubjectExam { get; set; }
 //       [Range(typeof(decimal), "0.0", "20.0")]