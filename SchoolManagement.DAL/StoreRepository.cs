using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SchoolManagement.Model;

namespace SchoolManagement.DAL
{
    public class StoreRepository : GenericRepository<Store>
    {
        public StoreRepository(smContext context)
            : base(context)
        { }

        public  void UpdateStore(Store entityToUpdate, Store OldStore)
        {
           // dbSet.Attach(entityToUpdate);
            //context.Entry(entityToUpdate).State = EntityState.Modified;
            // context.ObjectStateManager.ChangeObjectState(entityToUpdate, System.Data.EntityState.Modified);
            //  context.Entry();

            var entry = context.Entry(OldStore);
            entry.CurrentValues.SetValues(entityToUpdate);
            context.SaveChanges();

        }

    }
}
