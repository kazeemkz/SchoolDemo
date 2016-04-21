using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Model
{
   public class SecretCode
    {
        public int SecretCodeID { get; set; }
       [Required]
        public string Code { get; set; }
        public int UsedFrequency { get; set; }
        public DateTime CreationDate { get; set; }
        public bool GivenOut { get; set; }
    }
}
