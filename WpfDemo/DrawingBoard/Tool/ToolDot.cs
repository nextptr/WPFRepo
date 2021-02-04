using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using DrawingBoard.Primitive;


namespace DrawingBoard.Tool
{
    public class ToolDot : ITool
    {
        private Dot _dot;

        public void MouseDown(Panel canvas, MouseButtonEventArgs e, Matrix screenToWorld)
        {
            Point p = e.GetPosition(canvas);
            p = screenToWorld.Transform(p);

            _dot = new Dot();
            _dot.X = p.X;
            _dot.Y = p.Y;

            canvas.Children.Add(_dot);
        }

        public void MouseMove(Panel canvas, MouseEventArgs e, Matrix screenToWorld)
        {
        }

        public void MouseUp(Panel canvas, MouseButtonEventArgs e, Matrix screenToWorld)
        {
            if (_dot == null)
            {
                return;
            }

            _dot.IsSelected = true;
            _dot = null;
        }

        public void MouseDownPolyLine(Panel canvas, MouseButtonEventArgs e, Matrix screenToWorld, PolyLine1 ply)
        {
            return;
        }

        public void MouseRightDown(Panel canvas, MouseButtonEventArgs e, Matrix screenToWorld,PolyLine1 ply)
        {
           
        }
    }
}
