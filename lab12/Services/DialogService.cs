using System.Windows;

namespace lab12.Services;

/// <summary>
/// WPF-реализация сервиса диалогов. Только этот класс напрямую использует MessageBox.
/// </summary>
public sealed class DialogService : IDialogService
{
    public void ShowInfo(string message, string title = "Информация")
    {
        // Информационное сообщение используется после успешного добавления или сохранения контакта.
        MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Information);
    }

    public void ShowWarning(string message, string title = "Предупреждение")
    {
        // Предупреждение показывает некритичную проблему, например дубликат номера.
        MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Warning);
    }

    public void ShowError(string message, string title = "Ошибка")
    {
        // Ошибка показывает нарушение правил, которое не удалось обработать как обычное предупреждение.
        MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Error);
    }

    public bool ShowConfirmation(string message, string title = "Подтверждение")
    {
        // Метод возвращает bool, чтобы ViewModel могла принять решение без знания о MessageBoxResult.
        return MessageBox.Show(message, title, MessageBoxButton.YesNo, MessageBoxImage.Question)
            == MessageBoxResult.Yes;
    }
}
