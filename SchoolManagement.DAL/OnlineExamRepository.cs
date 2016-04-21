using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SchoolManagement.Model;

namespace SchoolManagement.DAL
{
  public  class OnlineExamRepository : GenericRepository<OnlineExam>
    {
      public OnlineExamRepository(smContext context)
          : base(context)
      {

      }
    }
}
