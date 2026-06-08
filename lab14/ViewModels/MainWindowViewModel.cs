using System.Windows.Input;
using lab14.Services;

namespace lab14.ViewModels;

/// <summary>
/// ViewModel оболочки: команды меню переключают текущий экран через INavigationService.
/// </summary>
public sealed class MainWindowViewModel
{
    private readonly INavigationService _navigation;

    public MainWindowViewModel(INavigationService navigation)
    {
        _navigation = navigation ?? throw new ArgumentNullException(nameof(navigation));

        // Свойство открыто для Binding в MainWindow.xaml:
        // ContentControl обращается к NavigationService.CurrentViewModel.
        NavigationService = _navigation;

        // Команды меню не знают о View. Они только просят сервис перейти к нужной ViewModel.
        ShowContactsCommand = new RelayCommand(() => _navigation.NavigateTo<ContactsListViewModel>());
        ShowAboutCommand = new RelayCommand(() => _navigation.NavigateTo<AboutViewModel>());

        // Стартовый экран задается в ViewModel оболочки, а не через StartupUri или Frame.
        _navigation.NavigateTo<ContactsListViewModel>();
    }

    public INavigationService NavigationService { get; }

    public ICommand ShowContactsCommand { get; }

    public ICommand ShowAboutCommand { get; }
}
