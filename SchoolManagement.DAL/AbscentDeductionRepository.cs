using SchoolManagement.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.DAL
{
    public class AbscentDeductionRepository : GenericRepository<AbscentDeduction>
    {
        public AbscentDeductionRepository(smContext context)
            : base(context)
        {

        }
    }
}
