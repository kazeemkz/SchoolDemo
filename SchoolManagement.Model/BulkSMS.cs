using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Model
{
    public class BulkSMS
    {
        public int BulkSMSID { get; set; }
        [Required]
        [StringLength(160, ErrorMessage = "The {0} must be at least {2} characters long and maximum of 160.", MinimumLength = 6)]
        public string Body { get; set; }
        public string Numbers { get; set; }
       // public string Role { get; set; }
        public string ClassArm { get; set; }
        public string Class { get; set; }
    }
}
