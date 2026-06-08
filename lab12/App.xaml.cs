using System.IO;
using System.Windows;
using lab12.Data;
using lab12.Services;
using lab12.ViewModels;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace lab12
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

            var databasePath = Path.Combine(
                AppContext.BaseDirectory,
                "Data",
                "PhoneBookDB_ФАМИЛИЯ_ГРУППА.db");
            var connectionString = new SqliteConnectionStringBuilder
            {
                DataSource = databasePath
            }.ToString();

            services.AddDbContext<PhoneBookDbContext>(
                options => options.UseSqlite(connectionString),
                ServiceLifetime.Singleton,
                ServiceLifetime.Singleton);

            // Singleton: репозиторий хранит общую коллекцию контактов на все переходы и синхронизирует ее с SQLite.
            services.AddSingleton<IContactsRepository, EfContactsRepository>();

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
