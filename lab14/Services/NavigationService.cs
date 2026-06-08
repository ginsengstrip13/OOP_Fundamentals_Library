using lab14.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace lab14.Services;

/// <summary>
/// Получает ViewModel из DI-контейнера и сообщает Shell, какой экран нужно показать.
/// </summary>
public sealed class NavigationService : ObservableObject, INavigationService
{
    private readonly IServiceProvider _serviceProvider;
    private object? _currentViewModel;

    public NavigationService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
    }

    public object? CurrentViewModel
    {
        get => _currentViewModel;
        // Set вызывает PropertyChanged. Благодаря этому Binding в ContentControl узнает,
        // что нужно заново подобрать DataTemplate и перерисовать область контента.
        private set => Set(ref _currentViewModel, value);
    }

    public void NavigateTo<TViewModel>(object? parameter = null)
        where TViewModel : class
    {
        // ViewModel создается контейнером, а не вручную через new.
        // Поэтому все зависимости выбранного экрана также будут подставлены автоматически.
        var viewModel = _serviceProvider.GetRequiredService<TViewModel>();

        // Передача параметра не обязательна для всех экранов.
        // Если ViewModel реализует INavigationAware, она сама решает, как обработать parameter.
        if (viewModel is INavigationAware navigationAware)
        {
            navigationAware.OnNavigatedTo(parameter);
        }

        // После этой строки Shell увидит новую CurrentViewModel и заменит содержимое ContentControl.
        CurrentViewModel = viewModel;
    }
}
