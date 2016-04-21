using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SchoolManagement.Model;

namespace SchoolManagement.DAL
{
    public class PrimarySchoolStudentRepository : GenericRepository<PrimarySchoolStudent>
    {
        public PrimarySchoolStudentRepository(smContext context)
          : base(context)
      {

      }
    }
}
