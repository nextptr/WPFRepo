using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using DrawingBoard.Primitive;

namespace DrawingBoard.Tool
{
    public class ToolRectangle : ITool
    {
        private Rectangle _rectangle;

        public void MouseDown(Panel canvas, MouseButtonEventArgs e, Matrix screenToWorld)
        {
            Point p = e.GetPosition(canvas);
            p = screenToWorld.Transform(p);

            _rectangle = new Rectangle();
            _rectangle.Left = p.X;
            _rectangle.Top = p.Y;
            _rectangle.Width = 0;
            _rectangle.Height = 0;

            canvas.Children.Add(_rectangle);
        }

        public void MouseMove(Panel canvas, MouseEventArgs e, Matrix screenToWorld)
        {
            if (_rectangle == null)
            {
                return;
            }

            Point p = e.GetPosition(canvas);
            p = screenToWorld.Transform(p);

            _rectangle.Width = p.X- _rectangle.Left;
            _rectangle.Height = -(p.Y - _rectangle.Top);
        }

        public void MouseUp(Panel canvas, MouseButtonEventArgs e, Matrix screenToWorld)
        {
            if (_rectangle == null)
            {
                return;
            }

            _rectangle.IsSelected = true;
            _rectangle = null;
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
