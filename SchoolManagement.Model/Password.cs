using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Model
{
  public  class Password
    {

        [Key]
      public int PasswordID { get; set; }

        public string Code { get; set; }

        public DateTime DateCreated { get; set; }

        public bool GivenOut { get; set; }
     
    }
}
