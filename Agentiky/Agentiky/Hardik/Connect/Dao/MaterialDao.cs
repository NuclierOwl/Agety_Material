using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agents_BD_Tres.Hardik.Connect.Dao
{
    class MaterialDao
    {
        public int id { get; set; }
        public string title { get; set; }
        public int countinpack { get; set; }
        public string unit { get; set; }
        public int countinstock { get; set; }
        public int mincount { get; set; }
        public string description { get; set; }
        public string image { get; set; }
        public int materialtypeid { get; set; }
    }
}
