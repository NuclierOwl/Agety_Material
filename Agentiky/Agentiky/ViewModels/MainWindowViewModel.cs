using Agentiky.Hardik.Dop;
using Agents_BD_Tres.Hardik.Connect.Dao;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Agentiky.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public List<AgentTypeDao> AgentTypes { get; set; }
        public List<string> SortOptions { get; } = new List<string> { "Наименование", "Размер скидки", "Приоритет" };
        public IEnumerable<ProductDao> Products { get; }
            = new ObservableCollection<ProductDao>();

        public MainWindowViewModel()
        {
            var dbService = new UpravaOgentov();
            Products = dbService.GetAllProducts().ToList();
        }
    }
}
