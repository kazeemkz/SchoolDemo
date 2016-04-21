using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Model
{
   public class Choice
    {
       [Key]
     //  [DatabaseGenerated(DatabaseGeneratedOption.None)]
       public int ChoiceID { get; set; }
       public string Text { get; set; }
       public bool IsAnswer { get; set; }
       public bool IsSelected { get; set; }
       public virtual Question Question { get; set; }
       public int QuestionID { get; set; }
    }
}
