using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Model
{
  public  class SubjectRegistration
    {
      [Key]
      //[DatabaseGenerated(DatabaseGeneratedOption.None)]
      public int SubjectRegistrationID { get; set; }
      public string Level { get; set; }
      public ICollection<Subject> Subjects { get; set; }


    }

}
