using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SchoolManagement.Model;

namespace SchoolManagement.DAL
{
   public class QuestionRepository  : GenericRepository<Question>
    {
       public QuestionRepository(smContext context)
          : base(context)
      {

      }
    }
}
