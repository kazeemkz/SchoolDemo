using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Model
{
    public class Blog
    {

        public Blog()
        {

        }
        public int BlogID { get; set; }
     //   public string Title { get; set; }
       // public string BloggerName { get; set; }
     //  public DateTime DateCreated { get; set; }
        public virtual ICollection<Post> Posts { get; set; }
    }

 
}
