using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Model
{
    public class StudentActivity : Activity
    {
      // public int StudentActivityID { get; set; }
       public string Name { get; set; }
       //public virtual MyRole MyRole { get; set; }
       public int MyRoleID { get; set; }
    }
}
