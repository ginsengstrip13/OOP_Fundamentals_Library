using System.Windows;
using lab11.Services;
using lab11.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace lab11
{
    /// <summary>
    /// Точка входа приложения. Здесь настраивается IoC-контейнер и запускается Shell.
    /// </summary>
    public partial class App : Application
    {
        private ServiceProvider? _serviceProvider;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var services = new ServiceCollection();

            // Singleton: репозиторий хранит общую коллекцию контактов на все переходы.
            // Экранные ViewModel создаются заново, но данные телефонной книги не теряются.
            services.AddSingleton<IContactsRepository, InMemoryContactsRepository>();

            // Singleton: сервис диалогов не хранит состояние и переиспользуется всеми ViewModel.
            services.AddSingleton<IDialogService, DialogService>();

            // Singleton: сервис навигации хранит CurrentViewModel, поэтому живет все время работы Shell.
            services.AddSingleton<INavigationService, NavigationService>();

            // Transient: каждый переход получает новый экземпляр экранной ViewModel с зависимостями из DI.
            services.AddTransient<ContactsListViewModel>();
            services.AddTransient<ContactEditViewModel>();
            services.AddTransient<AboutViewModel>();

            // Singleton: ViewModel оболочки одна, как и главное окно приложения.
            services.AddSingleton<MainWindowViewModel>();

            // Singleton: главное окно существует в одном экземпляре, DataContext назначает контейнер.
            services.AddSingleton<MainWindow>(serviceProvider =>
            {
                return new MainWindow
                {
                    DataContext = serviceProvider.GetRequiredService<MainWindowViewModel>()
                };
            });

            _serviceProvider = services.BuildServiceProvider();
            _serviceProvider.GetRequiredService<MainWindow>().Show();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            _serviceProvider?.Dispose();
            base.OnExit(e);
        }
    }
}
