namespace Agents_BD_Tres.Hardik.Connect.Dao
{
    public class ProductDao
    {
        public int id { get; set; }
        public string title { get; set; }
        public int? producttypeid { get; set; }
        public string articlenumber { get; set; }
        public string description { get; set; }
        public string image { get; set; }
        public int? productionpersoncount { get; set; }
        public int? productionworkshopnumber { get; set; }
        public decimal mincostforagent { get; set; } 
    }
}
