namespace lab11.Services;

/// <summary>
/// Абстракция диалоговых окон. ViewModel зависит от интерфейса, а не от MessageBox.
/// </summary>
public interface IDialogService
{
    void ShowInfo(string message, string title = "Информация");

    void ShowWarning(string message, string title = "Предупреждение");

    void ShowError(string message, string title = "Ошибка");

    bool ShowConfirmation(string message, string title = "Подтверждение");
}
