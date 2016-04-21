using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SchoolManagement.Model;

namespace SchoolManagement.DAL
{
    public class OrderItemRepository : GenericRepository<OrderItem>
    {
        public OrderItemRepository(smContext context)
          : base(context)
      {

      }
    }
}
