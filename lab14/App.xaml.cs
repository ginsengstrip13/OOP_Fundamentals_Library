using System.IO;
using System.Windows;
using lab14.Data;
using lab14.Services;
using lab14.ViewModels;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace lab14;

public partial class App : Application
{
    private ServiceProvider? _serviceProvider;

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        var services = new ServiceCollection();

        var databasePath = Path.Combine(
            System.AppContext.BaseDirectory,
            "Data",
            "PhoneBookDB_ФАМИЛИЯ_ГРУППА.db");
        var connectionString = new SqliteConnectionStringBuilder
        {
            DataSource = databasePath
        }.ToString();

        services.AddDbContextFactory<ApplicationContext>(
            options => options.UseSqlite(connectionString));

        services.AddSingleton<IDialogService, DialogService>();
        services.AddSingleton<INavigationService, NavigationService>();

        services.AddTransient<ContactsListViewModel>();
        services.AddTransient<ContactEditViewModel>();
        services.AddTransient<AboutViewModel>();
        services.AddSingleton<MainWindowViewModel>();

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
