using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SchoolManagement.Model;

namespace SchoolManagement.DAL
{
    public class ActivityRepository : GenericRepository<Activity>
    {
        public ActivityRepository(smContext context)
            : base(context)
        {

        }

    }
}
