namespace lab10.Services;

/// <summary>
/// Абстракция сервиса диалогов.
/// ViewModel зависит от интерфейса, а не от MessageBox, поэтому остается слабосвязанной с WPF UI.
/// </summary>
public interface IDialogService
{
    void ShowInfo(string message, string title = "Информация");

    void ShowWarning(string message, string title = "Предупреждение");

    void ShowError(string message, string title = "Ошибка");

    bool ShowConfirmation(string message, string title = "Подтверждение");
}
