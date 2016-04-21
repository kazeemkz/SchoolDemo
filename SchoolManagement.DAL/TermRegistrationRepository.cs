using SchoolManagement.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.DAL
{
    public class TermRegistrationRepository : GenericRepository<TermRegistration>
    {
      public TermRegistrationRepository(smContext context)
            : base(context)
        { }
    }
}
