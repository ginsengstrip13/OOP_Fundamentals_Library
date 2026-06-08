namespace lab14.Services;

/// <summary>
/// Позволяет ViewModel принять параметр при навигационном переходе.
/// </summary>
public interface INavigationAware
{
    // Метод вызывается сервисом навигации сразу после создания ViewModel из DI-контейнера.
    // Так экран может получить выбранный контакт, идентификатор записи или другой параметр перехода.
    void OnNavigatedTo(object? parameter);
}
