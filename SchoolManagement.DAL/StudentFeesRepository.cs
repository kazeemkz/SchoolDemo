using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SchoolManagement.Model;

namespace SchoolManagement.DAL
{
    public class StudentFeesRepository:  GenericRepository<StudentFees>
    {
         public StudentFeesRepository(smContext context)
          : base(context)
      {

      }
    }
}
