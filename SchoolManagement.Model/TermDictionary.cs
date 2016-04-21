using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SchoolManagement.Model
{
   public class TermDictionary
    {
        public static SelectList TermDicoList
        {
            get { return new SelectList(TermDic, "Value", "Key"); }
        }

        private static readonly IDictionary<string, string> TermDic = new Dictionary<string, string> 
       {{"Choose..",""} ,
        {"FIRST TERM","FIRST TERM"},
         {"SECOND TERM","SECOND TERM"},
          {"THIRD TERM","THIRD TERM"},
             
       };
    }
}
