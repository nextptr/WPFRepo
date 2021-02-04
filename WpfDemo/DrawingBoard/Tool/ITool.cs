using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using DrawingBoard.Primitive;

namespace DrawingBoard.Tool
{
    public interface ITool
    {
        void MouseDown(Panel canvas, MouseButtonEventArgs e, Matrix screenToWorld);
        void MouseMove(Panel canvas, MouseEventArgs e, Matrix screenToWorld);
        void MouseUp(Panel canvas, MouseButtonEventArgs e, Matrix screenToWorld);

        void MouseDownPolyLine(Panel canvas, MouseButtonEventArgs e, Matrix screenToWorld, PolyLine1 ply);
        void MouseRightDown(Panel canvas, MouseButtonEventArgs e, Matrix screenToWorld, PolyLine1 ply);
    }
}
