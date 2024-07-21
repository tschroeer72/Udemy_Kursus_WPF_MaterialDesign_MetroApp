using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikkiDBLib.Modells;

namespace WikkiDBLib.DBAccess
{
    public class DBUnit
    {
        public static DBAccessHelper<Person> Person 
        {
            get
            {
                return new DBAccessHelper<Person>();
            }
        }

        public static DBAccessHelper<Stadt> Stadt
        {
            get
            {
                return new DBAccessHelper<Stadt>();
            }
        }


    }
}
