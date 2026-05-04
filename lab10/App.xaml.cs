using System.Windows;
using lab10.Services;
using lab10.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace lab10
{
    /// <summary>
    /// Точка входа приложения.
    /// Здесь создается IoC-контейнер, который управляет созданием сервисов, ViewModel и главного окна.
    /// </summary>
    public partial class App : Application
    {
        private ServiceProvider? _serviceProvider;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var services = new ServiceCollection();

            // IDialogService регистрируется как Singleton:
            // сервис диалогов не хранит пользовательское состояние, поэтому одному экземпляру
            // достаточно обслуживать все окно на протяжении работы приложения.
            services.AddSingleton<IDialogService, DialogService>();

            // MainViewModel регистрируется как Transient:
            // при каждом запросе контейнер создает новый экземпляр ViewModel с актуальными зависимостями.
            services.AddTransient<MainViewModel>();

            // MainWindow регистрируется как Singleton:
            // главное окно в приложении одно, а DataContext назначается через контейнер,
            // чтобы окно не создавало ViewModel напрямую и не знало о ее зависимостях.
            services.AddSingleton<MainWindow>(serviceProvider =>
            {
                var window = new MainWindow
                {
                    DataContext = serviceProvider.GetRequiredService<MainViewModel>()
                };

                return window;
            });

            _serviceProvider = services.BuildServiceProvider();

            var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            _serviceProvider?.Dispose();
            base.OnExit(e);
        }
    }
}
