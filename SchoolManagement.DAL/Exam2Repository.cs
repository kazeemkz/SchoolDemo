using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SchoolManagement.Model;

namespace SchoolManagement.DAL
{
    public class Exam2Repository : GenericRepository<Exam2>
    {
        public Exam2Repository(smContext context)
            : base(context)
        {

        }
    }
}
