using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using DrawingBoard.Serializables;

namespace DrawingBoard.Primitive
{
    public class Line : PrimitiveBase
    {
        public double X1
        {
            get
            {
                return (double)base.GetValue(Line.X1Property);
            }
            set
            {
                base.SetValue(Line.X1Property, value);
            }
        }
        public double Y1
        {
            get
            {
                return (double)base.GetValue(Line.Y1Property);
            }
            set
            {
                base.SetValue(Line.Y1Property, value);
            }
        }
        public double X2
        {
            get
            {
                return (double)base.GetValue(Line.X2Property);
            }
            set
            {
                base.SetValue(Line.X2Property, value);
            }
        }
        public double Y2
        {
            get
            {
                return (double)base.GetValue(Line.Y2Property);
            }
            set
            {
                base.SetValue(Line.Y2Property, value);
            }
        }

        protected override Geometry DefiningGeometry
        {
            get
            {
                return GetDefiningGeometry();
            }
        }

        public static readonly DependencyProperty X1Property = DependencyProperty.Register("X1", typeof(double), typeof(Line), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender));
        public static readonly DependencyProperty Y1Property = DependencyProperty.Register("Y1", typeof(double), typeof(Line), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender));
        public static readonly DependencyProperty X2Property = DependencyProperty.Register("X2", typeof(double), typeof(Line), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender));
        public static readonly DependencyProperty Y2Property = DependencyProperty.Register("Y2", typeof(double), typeof(Line), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender));

        public Line()
        {
           
        }

        protected override void OnMoveToOrigin()
        {
            double x = (X1 + X2) / 2;
            double y = (Y1 + Y2) / 2;

            X1 += -x;
            Y1 += -y;
            X2 += -x;
            Y2 += -y;
        }

        /// <summary>
        /// 移动拉伸图
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPrimitiveMouseMove(MouseEventArgs e)
        {
            Point pt = e.GetPosition(this);
            double dx = pt.X - _last.X;
            double dy = pt.Y - _last.Y;
            _isChange = 0;
            
            if (_handle == 0)
            {
                X1 += dx;
                Y1 += dy;
                X2 += dx;
                Y2 += dy;
                _isChange = 1;
            }
            else if (_handle == 1)
            {
                X1 += dx;
                Y1 += dy;
                _isChange = 1;
            }
            else
            {
                X2 += dx;
                Y2 += dy;
                _isChange = 1;
            }

            _last = pt;
          //  PrimitiveBase prim = new Line();
          //  _stk.Push(prim);
        }

        /// <summary>
        /// 拉伸图鼠标按下的时候事件
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPrimitiveMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            _last = e.GetPosition(this);
            Rect r1 = GetHandleRect(X1, Y1);
            Rect r2 = GetHandleRect(X2, Y2);
            if (r1.Contains(_last))
            {
                _handle = 1;
            }
            else if (r2.Contains(_last))
            {
                _handle = 2;
            }
            else
            {
                _handle = 0;
            }
        }

        private Geometry GetDefiningGeometry()
        {
            GeometryGroup gg = new GeometryGroup();

            LineGeometry lineGeometry = new LineGeometry();
            lineGeometry.StartPoint = new Point(X1, Y1);
            lineGeometry.EndPoint = new Point(X2, Y2);



            gg.Children.Add(lineGeometry);

            if (IsSelected)
            {
                RectangleGeometry[] rectGeomety = new RectangleGeometry[2] { new RectangleGeometry(), new RectangleGeometry() };

                rectGeomety[0].Rect = GetHandleRect(X1, Y1);
                rectGeomety[1].Rect = GetHandleRect(X2, Y2);

                gg.Children.Add(rectGeomety[0]);
                gg.Children.Add(rectGeomety[1]);
            }

            return gg;
        }

        protected override void OnRender(DrawingContext drawingContext)
        {

            Pen pen = new Pen(Stroke, ActualStrokeThickness);
            Point startPoint = new Point(X1, Y1);
            Point endPoint = new Point(X2, Y2);

            drawingContext.DrawLine(pen, startPoint, endPoint);

            if (IsSelected)
            {
                DrawHandle(drawingContext, GetHandleRect(X1, Y1));
                DrawHandle(drawingContext, GetHandleRect(X2, Y2));
            }
            else
            {

            }
            // call base.OnRender will invoke DefiningGeometry
            //base.OnRender(drawingContext);
        }

        protected override void OnMoved(EventArgs e)
        {
            base.OnMoved(e);
        }

        public override SerializableBase CreateSerializableObject()
        {
            SerializableLine line = new SerializableLine(this);
            return line;
        }

        public override object Clone()
        {
            Line clone = new Line();
            clone.X1 = this.X1;
            clone.Y1 = this.Y1;
            clone.X2 = this.X2;
            clone.Y2 = this.Y2;
            return clone;
        }

        public override void Move(double x, double y)
        {
            this.X1 += x;
            this.X2 += x;
            this.Y1 += y;
            this.Y2 += y;
        }
    }
}
