using System.Windows.Controls;

namespace lab14.Views;

/// <summary>
/// Представление списка контактов. Логика находится в ContactsListViewModel.
/// </summary>
public partial class ContactsListView : UserControl
{
    public ContactsListView()
    {
        // Представление только загружает XAML. DataContext назначается через DataTemplate.
        InitializeComponent();
    }
}
