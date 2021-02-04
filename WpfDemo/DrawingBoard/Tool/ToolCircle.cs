using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using DrawingBoard.Primitive;

namespace DrawingBoard.Tool
{
    public class ToolCircle : ITool
    {
        private Circle _circle;

        public void MouseDown(Panel canvas, MouseButtonEventArgs e, Matrix screenToWorld)
        {
            Point p = e.GetPosition(canvas);
            p = screenToWorld.Transform(p);

            _circle = new Circle();
            _circle.X = p.X;
            _circle.Y = p.Y;
            _circle.Radius = 0;

            canvas.Children.Add(_circle);
        }

        public void MouseMove(Panel canvas, MouseEventArgs e, Matrix screenToWorld)
        {
            if (_circle == null)
            {
                return;
            }

            Point p = e.GetPosition(canvas);
            p = screenToWorld.Transform(p);

            _circle.Radius = p.X- _circle.X;
        }

        public void MouseUp(Panel canvas, MouseButtonEventArgs e, Matrix screenToWorld)
        {
            if (_circle == null)
            {
                return;
            }

            _circle.IsSelected = true;
            _circle = null;
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
