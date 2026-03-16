using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace lab3
{
    public partial class MainWindow : Window
    {
        private readonly List<ShapeCreator> _creators;

        public MainWindow()
        {
            InitializeComponent();

            _creators = new List<ShapeCreator>
            {
                new CircleCreator(),
                new SquareCreator(),
                new TriangleCreator()
            };
        }

        private void ColorSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DrawingCanvas == null) return;

            DrawingCanvas.Children.Clear();

            var selectedItem = (ComboBoxItem)ColorSelector.SelectedItem;
            if (selectedItem?.Tag == null) return;

            string colorTag = selectedItem.Tag.ToString();
            Brush selectedBrush = (Brush)new BrushConverter().ConvertFromString(colorTag);

            double currentX = 50;
            double currentY = 50;

            foreach (var creator in _creators)
            {
                IShape shape = creator.CreateShape();
                shape.Draw(DrawingCanvas, selectedBrush, currentX, currentY);
                currentX += 100;
            }
        }
    }
}