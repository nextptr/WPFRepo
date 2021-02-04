using System.Windows;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows.Input;
using DrawingBoard.Primitive;
namespace DrawingBoard.Tool
{
   public class ToolPolyLine : ITool
    {
        private PolyLine1 _polyline;
        private System.Windows.Shapes.Line _line;
        //private Line _line;
        private int lastIndex = 0;

        public void MouseDown(Panel canvas, MouseButtonEventArgs e, Matrix screenToWorld)
        {
           
        }

        public void MouseDownPolyLine(Panel canvas, MouseButtonEventArgs e, Matrix screenToWorld, PolyLine1 ply)
        {
            Point p = e.GetPosition(canvas);
            p = screenToWorld.Transform(p);
            var point = new Point(p.X, p.Y);
            
            ply.Points.Add(point);

            _polyline = ply;
            PolyLine1 clonePy = new PolyLine1();
            clonePy.Points = new PointCollection();

            _line = new System.Windows.Shapes.Line();
            //_line = new Line();
            _line.Stroke = new SolidColorBrush(Colors.Black);
            _line.StrokeDashArray.Add(4);
            _line.X1 = p.X;
            _line.Y1 = p.Y;
            _line.X2 = p.X;
            _line.Y2 = p.Y;


            if (ply.Points.Count == 1)
            {
                // ply.Points.Add(point);//添加新的数据点
                canvas.Children.Add(ply);
                canvas.Children.Add(_line);
                lastIndex= canvas.Children.IndexOf(ply);
            }
            else
            {
                canvas.Children.RemoveAt(lastIndex);
                clonePy.Points = ply.Points.Clone();
                _polyline = clonePy;
                canvas.Children.Insert(lastIndex, clonePy);//重新new  新节点否则不会onrender 执行
                canvas.Children.RemoveAt(lastIndex + 1);
                canvas.Children.Insert(lastIndex + 1, _line);
            }
        }
        public void MouseMove(Panel canvas, MouseEventArgs e, Matrix screenToWorld)
        {
            if (_polyline == null)
            {
                return;
            }

            Point p = e.GetPosition(canvas);
            p = screenToWorld.Transform(p);

            if (_polyline.Points.Count > 0)
            {
                var point = _polyline.Points[_polyline.Points.Count - 1];
                _line.X1 = point.X;
                _line.Y1 = point.Y;

                _line.X2 = p.X;
                _line.Y2 = p.Y;
               // _polyline.Points.Add(new Point(p.X, p.Y));
            }
        }

        public void MouseUp(Panel canvas, MouseButtonEventArgs e, Matrix screenToWorld)
        {
            if (_polyline == null)
            {
                return;
            }

            //_polyline.IsSelected = true;
        }

        public void MouseRightDown(Panel canvas, MouseButtonEventArgs e, Matrix screenToWorld, PolyLine1 ply)
        {
            if (_polyline == null)
            {
                return;
            }

            _polyline.IsSelected = true;
            //_polyline.Points.Clear();
           // ply.Points.Clear();
            _line.X1 = _line.X2 = _line.Y1 = _line.Y2 = 0;
            
        }
    }
}
