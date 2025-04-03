using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agents_BD_Tres.Hardik.Connect.Dao
{
    public class AgentDao
    {

        public int id { get; set; }
        public string title { get; set; }
        public int agenttypeid { get; set; }
        public string address { get; set; }
        public string inn { get; set; }
        public string kpp { get; set; }
        public string directorname { get; set; }
        public string phone { get; set; }
        public string email { get; set; }
        public string logo { get; set; }
        public int priority { get; set; }
    }
}
