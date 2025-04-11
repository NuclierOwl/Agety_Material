using Avalonia.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using Agents_BD_Tres.Hardik.Connect;
using Agents_BD_Tres.Hardik.Connect.Dao;
using Avalonia.Data;
using Agentiky.ViewModels;

namespace Agentiky.Views
{
    public partial class MainWindow : Window
    {
        private readonly DatabaseConnection _dbConnection;
        private List<AgentDao> _allAgents;
        private List<AgentTypeDao> _agentTypes;
        private int _currentPage = 1;
        private const int ItemsPerPage = 10;
        private string _searchTerm = "";
        private string _sortField = "Title";
        private bool _sortAscending = true;
        private int? _filterTypeId = null;

        public List<string> SortOptions { get; } = new List<string> { "Наименование", "Размер скидки", "Приоритет" };

        public MainWindow()
        {
            InitializeComponent();
            _dbConnection = new DatabaseConnection();
            LoadData();
            InitializeEvents();
        }

        private void LoadData()
        {
            _allAgents = _dbConnection.GetAllAgents();
            _agentTypes = _dbConnection.GetAgentTypes();
            _agentTypes.Insert(0, new AgentTypeDao { id = 0, title = "Все типы", image = null });
            ApplyFiltersAndSort();
        }

        private void InitializeEvents()
        {
            SearchBox.TextChanged += (s, e) =>
            {
                _searchTerm = SearchBox.Text ?? "";
                _currentPage = 1;
                ApplyFiltersAndSort();
            };

            TypeComboBox.SelectionChanged += (s, e) =>
            {
                _filterTypeId = TypeComboBox.SelectedIndex == 0 ? null : ((AgentTypeDao)TypeComboBox.SelectedItem).id;
                _currentPage = 1;
                ApplyFiltersAndSort();
            };

            SortComboBox.SelectionChanged += (s, e) =>
            {
                _sortField = SortComboBox.SelectedIndex switch
                {
                    0 => "Title",
                    1 => "Discount",
                    2 => "Priority",
                    _ => "Title"
                };
                ApplyFiltersAndSort();
            };

            SortDirectionButton.Click += (s, e) =>
            {
                _sortAscending = !_sortAscending;
                ApplyFiltersAndSort();
            };

            PrevButton.Click += (s, e) =>
            {
                if (_currentPage > 1)
                {
                    _currentPage--;
                    ApplyFiltersAndSort();
                }
            };

            NextButton.Click += (s, e) =>
            {
                if (_currentPage < GetTotalPages())
                {
                    _currentPage++;
                    ApplyFiltersAndSort();
                }
            };

            ChangePriorityButton.Click += async (s, e) =>
            {
                var selectedAgents = AgentsListBox.SelectedItems.Cast<AgentViewModel>().Select(x => x.Agent).ToList();
                var maxPriority = selectedAgents.Max(a => a.priority);

                var dialog = new ChangeDialog(maxPriority);
                var result = await dialog.ShowDialog<bool>(this);

                if (result)
                {
                    int newPriority = dialog.NewPriority;
                    foreach (var agent in selectedAgents)
                    {
                        agent.priority = newPriority;
                        _dbConnection.UpdateAgent(agent);
                    }
                    LoadData();
                }
            };

            AgentsListBox.SelectionChanged += (s, e) =>
            {
                ChangePriorityButton.IsEnabled = AgentsListBox.SelectedItems.Count > 0;
            };
        }

        private void ApplyFiltersAndSort() //фильтры
        {
            var filteredAgents = _allAgents.AsQueryable();

            if (!string.IsNullOrEmpty(_searchTerm))
            {
                filteredAgents = filteredAgents.Where(a =>
                    a.title.Contains(_searchTerm, StringComparison.OrdinalIgnoreCase) ||
                    (a.phone != null && a.phone.Contains(_searchTerm)) ||
                    (a.email != null && a.email.Contains(_searchTerm, StringComparison.OrdinalIgnoreCase)));
            }

            if (_filterTypeId.HasValue)
            {
                filteredAgents = filteredAgents.Where(a => a.agenttypeid == _filterTypeId.Value);
            }

            var agentsWithStats = filteredAgents.Select(a => new
            {
                Agent = a,
                AgentType = _agentTypes.FirstOrDefault(t => t.id == a.agenttypeid),
                SalesCount = _dbConnection.GetAgentSalesCount(a.id, DateTime.Now.AddYears(-1), DateTime.Now),
                TotalSalesAmount = _dbConnection.GetAgentTotalSalesAmount(a.id)
            }).ToList();

            var agentsWithDiscount = agentsWithStats.Select(x => new AgentViewModel
            {
                Agent = x.Agent,
                AgentType = x.AgentType,
                SalesCount = x.SalesCount,
                Discount = CalculateDiscount(x.TotalSalesAmount)
            }).ToList();

            var sortedAgents = _sortField switch
            {
                "Title" => _sortAscending ?
                    agentsWithDiscount.OrderBy(a => a.Agent.title) :
                    agentsWithDiscount.OrderByDescending(a => a.Agent.title),
                "Discount" => _sortAscending ?
                    agentsWithDiscount.OrderBy(a => a.Discount) :
                    agentsWithDiscount.OrderByDescending(a => a.Discount),
                "Priority" => _sortAscending ?
                    agentsWithDiscount.OrderBy(a => a.Agent.priority) :
                    agentsWithDiscount.OrderByDescending(a => a.Agent.priority),
                _ => agentsWithDiscount.OrderBy(a => a.Agent.title)
            };

            var pagedAgents = sortedAgents
                .Skip((_currentPage - 1) * ItemsPerPage)
                .Take(ItemsPerPage)
                .ToList();

            AgentsListBox.ItemsSource = pagedAgents;
            UpdatePagesButtons();
        }

        private void UpdatePagesButtons() //актуализация кнопок для страниц
        {
            var PagesPanel = (StackPanel)NextButton.Parent;


            while (PagesPanel.Children.Count > 2)    //удаление старых кнопок
            {
                PagesPanel.Children.RemoveAt(1);
            }


            for (int i = 1; i <= GetTotalPages(); i++)    //добавление новых кнопок
            {
                var pageButton = new Button { Content = i.ToString() };
                int page = i;
                pageButton.Click += (s, e) =>
                {
                    _currentPage = page;
                    ApplyFiltersAndSort();
                };
                PagesPanel.Children.Insert(PagesPanel.Children.Count - 1, pageButton);
            }
        }

        private int GetTotalPages() //кол-во страниц
        {
            return (int)Math.Ceiling(_allAgents.Count / (double)ItemsPerPage);
        }

        private int CalculateDiscount(decimal totalSales) //вычесление скидок
        {
            if (totalSales >= 500000) return 25;
            if (totalSales >= 150000) return 20;
            if (totalSales >= 50000) return 10;
            if (totalSales >= 10000) return 5;
            return 0;
        }
    }
}