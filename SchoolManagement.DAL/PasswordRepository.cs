using SchoolManagement.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.DAL
{
    public class PasswordRepository : GenericRepository<Password>
    {
        public PasswordRepository(smContext context)
            : base(context)
        { }
    }
}
