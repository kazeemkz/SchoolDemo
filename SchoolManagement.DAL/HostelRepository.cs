using SchoolManagement.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.DAL
{
    public class HostelRepository : GenericRepository<Hostel>
    {
        public HostelRepository(smContext context)
            : base(context)
        {




        }
    }
}
