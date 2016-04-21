using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SchoolManagement.Model;

namespace SchoolManagement.DAL
{
    public class Exam1Repository : GenericRepository<Exam1>
    {
        public Exam1Repository(smContext context)
          : base(context)
      {

      }
    }
}
