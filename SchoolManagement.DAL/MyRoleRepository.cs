using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SchoolManagement.Model;

namespace SchoolManagement.DAL
{
    public class MyRoleRepository : GenericRepository<MyRole>
    {

        public MyRoleRepository(smContext context)
          : base(context)
      {

      }
    }
}
