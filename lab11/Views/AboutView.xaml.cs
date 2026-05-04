using System.Windows.Controls;

namespace lab11.Views;

/// <summary>
/// Представление информационного экрана.
/// </summary>
public partial class AboutView : UserControl
{
    public AboutView()
    {
        // Code-behind не содержит логики: весь текст приходит из AboutViewModel.
        InitializeComponent();
    }
}
