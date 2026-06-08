namespace lab12.ViewModels;

/// <summary>
/// ViewModel информационного экрана.
/// </summary>
public sealed class AboutViewModel
{
    // Свойства только для чтения: экран "О программе" ничего не редактирует,
    // он просто демонстрирует навигацию к дополнительному представлению.
    public string AppName => "Телефонная книга MVVM";

    public string Version => "Версия 2.0: Shell и ViewModel-First навигация";

    public string Description => "Приложение демонстрирует навигацию через ContentControl, DataTemplate и DI-контейнер без Frame/Page.";
}
