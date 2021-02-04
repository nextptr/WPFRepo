using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

using DrawingBoard.Primitive;

namespace DrawingBoard.Tool
{
    public class ToolLine : ITool
    {
        private Line _line;

        public void MouseDown(Panel canvas, MouseButtonEventArgs e, Matrix screenToWorld)
        {
            Point p = e.GetPosition(canvas);
            p = screenToWorld.Transform(p);

            _line = new Line();
            _line.X1 = p.X;
            _line.Y1 = p.Y;
            _line.X2 = p.X;
            _line.Y2 = p.Y;


            //RotateTransform angle = new RotateTransform(); //旋转 
            //ScaleTransform scale = new ScaleTransform(); //缩放 
            //TransformGroup group = new TransformGroup();
            //angle.Angle = Math.PI / 6;
            //group.Children.Add(scale);
            //group.Children.Add(angle);

            //_line.RenderTransform = group;
          canvas.Children.Add(_line);
        }

        public void MouseMove(Panel canvas, MouseEventArgs e, Matrix screenToWorld)
        {
            if (_line == null)
            {
                return;
            }

            Point p = e.GetPosition(canvas);
            p = screenToWorld.Transform(p);

            _line.X2 = p.X;
            _line.Y2 = p.Y;
        }

        public void MouseUp(Panel canvas, MouseButtonEventArgs e, Matrix screenToWorld)
        {
            if (_line == null)
            {
                return;
            }

            _line.IsSelected = true;
            _line = null;
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
