using System.Windows.Controls;

namespace lab3
{
    public interface IShape
    {
        void Draw(Canvas canvas, double x, double y);
    }
}