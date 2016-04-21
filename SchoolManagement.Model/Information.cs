using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Model
{
    public class Information
    {
        [Key]
       // [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int InformationID { get; set; }
     //   [Required]
        public string To { get; set; }
        public string StudentID { get; set; }
        public string SentBy { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        [DataType(DataType.MultilineText)]
        public string Body { get; set; }
        public DateTime DateSent { get; set; }

    }
}
