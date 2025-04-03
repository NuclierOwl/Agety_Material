using Avalonia.Controls;
using Avalonia.Layout;

public class ChangeDialog : Window // 
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
        Title = "��������� ����������";

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

        var okButton = new Button { Content = "��������", Width = 100 };
        okButton.Click += (s, e) =>
        {
            NewPriority = (int)priorityTextBox.Value;
            Close(true);
        };

        var cancelButton = new Button { Content = "������", Width = 100 };
        cancelButton.Click += (s, e) => Close(false);

        stackPanel.Children.Add(new TextBlock
        {
            Text = "����� ���������:",
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
}