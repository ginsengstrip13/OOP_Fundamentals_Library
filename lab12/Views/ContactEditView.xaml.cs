using System.Windows.Controls;

namespace lab12.Views;

/// <summary>
/// Представление редактирования контакта.
/// </summary>
public partial class ContactEditView : UserControl
{
    public ContactEditView()
    {
        // Вся логика сохранения и отмены находится в ContactEditViewModel.
        InitializeComponent();
    }
}
