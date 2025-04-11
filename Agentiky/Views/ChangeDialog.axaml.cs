using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Layout;

namespace Agentiky.Views;

public partial class ChangeDialog : Window //изменение преоритетов
{
    public int NewPriority { get; private set; }

    public ChangeDialog(int initialPriority)
    {
        InitializeComponent(initialPriority);
    }

    private void InitializeComponent(int initialPriority)
    {
        Width = 300;
        Height = 200;
        Title = "Изменение приоритета";

        var stackPanel = new StackPanel
        {
            VerticalAlignment = VerticalAlignment.Center,
            HorizontalAlignment = HorizontalAlignment.Center,
            Spacing = 10
        };

        var priorityTextBox = new NumericUpDown
        {
            Value = initialPriority,
            Minimum = 0,
            Maximum = 100,
            Width = 100,
            HorizontalAlignment = HorizontalAlignment.Center
        };

        var okButton = new Button { Content = "Изменить", Width = 100 };
        okButton.Click += (s, e) =>
        {
            NewPriority = (int)priorityTextBox.Value;
            Close(true);
        };

        var cancelButton = new Button { Content = "Отмена", Width = 100 };
        cancelButton.Click += (s, e) => Close(false);

        stackPanel.Children.Add(new TextBlock
        {
            Text = "Новый приоритет:",
            HorizontalAlignment = HorizontalAlignment.Center
        });
        stackPanel.Children.Add(priorityTextBox);

        var buttonPanel = new StackPanel
        {
            Orientation = Orientation.Horizontal,
            HorizontalAlignment = HorizontalAlignment.Center,
            Spacing = 10
        };
        buttonPanel.Children.Add(okButton);
        buttonPanel.Children.Add(cancelButton);

        stackPanel.Children.Add(buttonPanel);

        Content = stackPanel;
    }

    public void OkButton_Click(object sender, RoutedEventArgs e)
    {
        NewPriority = (int)PriorityBox.Value;
        Close(true);
    }
    
    public void CancelButton_Click(object sender, RoutedEventArgs e)
    {
        Close(false);
    }
}