using SchoolManagement.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using eLibrary.Model;

namespace SchoolManagement.DAL
{
    public class AdditionalChapterTextRepository : GenericRepository<AdditionalChapterText>
    {
        public AdditionalChapterTextRepository(smContext context)
          : base(context)
      {

      }
    }
}
