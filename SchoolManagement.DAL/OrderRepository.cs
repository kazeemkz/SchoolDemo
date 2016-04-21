using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SchoolManagement.Model;

namespace SchoolManagement.DAL
{
    public class OrderRepository : GenericRepository<Order>
    {
       public OrderRepository(smContext context)
            : base(context)
        {

        }
    }
}
