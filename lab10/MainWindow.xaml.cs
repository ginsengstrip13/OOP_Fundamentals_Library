using System.Windows;

namespace lab10
{
    /// <summary>
    /// Code-behind главного окна содержит только инициализацию компонентов View.
    /// DataContext назначается в App.xaml.cs через контейнер зависимостей.
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
    }
}
