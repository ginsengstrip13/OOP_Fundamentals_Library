using System.Windows.Controls;
using System.Windows.Media;

namespace lab3
{
    public interface IShape
    {
        void Draw(Canvas canvas, Brush color, double x, double y);
    }
}