using System.Windows;
using System.Windows.Controls;

namespace lab3
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ColorSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DrawingCanvas == null) return;
            DrawingCanvas.Children.Clear();

            var selectedItem = (ComboBoxItem)ColorSelector.SelectedItem;
            if (selectedItem?.Tag == null) return;

            string colorTag = selectedItem.Tag.ToString();

            // 1. Выбираем нужную фабрику в зависимости от темы
            IShapeFactory factory = null;
            switch (colorTag)
            {
                case "Red":
                    factory = new RedShapeFactory();
                    break;
                case "Blue":
                    factory = new BlueShapeFactory();
                    break;
                case "Green":
                    factory = new GreenShapeFactory();
                    break;
            }

            if (factory == null) return;

            // 2. Клиентский код теперь работает ТОЛЬКО с интерфейсом абстрактной фабрики.
            // Форма больше не знает про классы цветов (Brushes), это инкапсулировано в фабриках.
            IShape circle = factory.CreateCircle();
            IShape square = factory.CreateSquare();
            IShape triangle = factory.CreateTriangle();

            // 3. Рисуем фигуры, просто передавая координаты
            circle.Draw(DrawingCanvas, 50, 50);
            square.Draw(DrawingCanvas, 150, 50);
            triangle.Draw(DrawingCanvas, 250, 50);
        }
    }
}