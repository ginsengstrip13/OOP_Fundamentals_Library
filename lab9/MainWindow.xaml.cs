using System.Windows;
using lab9.ViewModels;

namespace lab9
{
    /// <summary>
    /// Code-behind главного окна.
    /// В MVVM здесь остается только техническая инициализация View:
    /// создается MainViewModel и назначается DataContext для XAML-привязок.
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // DataContext связывает View с ViewModel.
            // После этого XAML может обращаться к Name, Phone, Contacts, AddCommand и DeleteCommand.
            DataContext = new MainViewModel();
        }
    }
}

