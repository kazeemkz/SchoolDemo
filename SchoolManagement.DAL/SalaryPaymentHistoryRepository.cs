using SchoolManagement.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.DAL
{
    public class SalaryPaymentHistoryRepository : GenericRepository<SalaryPaymentHistory>
    {
        public SalaryPaymentHistoryRepository(smContext context)
            : base(context)
        {

        }
    }
}
