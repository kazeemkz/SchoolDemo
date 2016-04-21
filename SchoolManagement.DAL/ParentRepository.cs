using SchoolManagement.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.DAL
{
   public class ParentRepository : GenericRepository<Parent>
    {
       public ParentRepository(smContext context)
            : base(context)
        {

        }
    }
}
