using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SchoolManagement.Model;

namespace SchoolManagement.DAL
{
    public class SubjectRepository : GenericRepository<Subject>
    {
        public SubjectRepository(smContext context)
            : base(context)
        {




        }
    }
}
