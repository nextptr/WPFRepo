using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using DrawingBoard.Primitive;

namespace DrawingBoard.Tool
{
    public class ToolPointer : ITool
    {
        public void MouseDown(Panel canvas, MouseButtonEventArgs e, Matrix screenToWorld)
        {

        }

        public void MouseMove(Panel canvas, MouseEventArgs e, Matrix screenToWorld)
        {

        }

        public void MouseUp(Panel canvas, MouseButtonEventArgs e, Matrix screenToWorld)
        {

        }

        public void MouseDownPolyLine(Panel canvas, MouseButtonEventArgs e, Matrix screenToWorld, PolyLine1 ply)
        {
            return;
        }

        public void MouseRightDown(Panel canvas, MouseButtonEventArgs e, Matrix screenToWorld, PolyLine1 ply)
        {

        }
    }
}
