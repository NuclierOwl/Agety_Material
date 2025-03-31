using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agents_BD_Tres.Hardik.Connect.Dao
{
    public class ProductDao
    {
        public int id { get; set; }
        public string title { get; set; }
        public int countinpack { get; set; }
        public string unit { get; set; }
        public int idcountinstock { get; set; }
        public float mincount { get; set; }
        public string description { get; set; }
        public int cost { get; set; }
        public string image { get; set; }
        public int materialtypeid { get; set; }
    }
}
