namespace lab14.Services;

/// <summary>
/// Сервис ViewModel-First навигации между экранами внутри Shell.
/// </summary>
public interface INavigationService
{
    // Shell привязывает ContentControl к этому свойству.
    // Тип object? выбран специально: текущим экраном может быть любая ViewModel приложения.
    object? CurrentViewModel { get; }

    // Обобщенный метод делает вызов навигации типобезопасным:
    // _navigation.NavigateTo<AboutViewModel>() нельзя случайно написать с неверной строкой-ключом.
    void NavigateTo<TViewModel>(object? parameter = null)
        where TViewModel : class;
}
