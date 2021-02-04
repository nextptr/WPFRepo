using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace DrawingBoard.DrawingControl
{
    public enum OriginPosition
    {
        TopLeft = 0,
        TopRight,
        BottomLeft,
        BottomRight
    };
    public class ZoomableCanvas : Canvas
    {
        public delegate void TransfromChangedEventHandler(object sender, EventArgs e);
        public event TransfromChangedEventHandler TransfromChanged;

        private OriginPosition _origin;
        public OriginPosition OriginPosition
        {
            get { return _origin; }
            set
            {
                if (_origin != value)
                {
                    _origin = value;
                    switch (_origin)
                    {
                        case OriginPosition.TopLeft:
                            break;
                        case OriginPosition.BottomLeft:
                            ScaleAt(1, -1, 0, 0);
                            break;
                        case OriginPosition.TopRight:
                            break;
                        case OriginPosition.BottomRight:
                            break;
                        default:
                            break;
                    }
                }
            }
        }
        public Matrix Matrix
        {
            get { return MatrixTransform.Matrix; }
            set { MatrixTransform.Matrix = value; }
        }
        private MatrixTransform MatrixTransform
        {
            get
            {
                return this.RenderTransform as MatrixTransform;
            }
        }
        public double OneUnitThickness
        {
            get
            {
                return (1 / Matrix.M11);
            }
        }

        public ZoomableCanvas()
        {
            // setting CacheMode can improve render performance
            OpenCacheMode(false);
            OpenAntialiasing(false);

            OriginPosition = OriginPosition.TopLeft;

            this.RenderTransform = new MatrixTransform();
        }

        public void OpenCacheMode(bool open) //像素对齐
        {
            if (open)
            {
                BitmapCache cache = new BitmapCache();
                cache.SnapsToDevicePixels = true;
                cache.RenderAtScale = 10;
                this.CacheMode = cache;
            }
            else
            {
                this.CacheMode = null;
            }
        }
        public void OpenAntialiasing(bool open) //反锯齿
        {
            if (open)
            {
                RenderOptions.SetEdgeMode(this, EdgeMode.Unspecified);
                RenderOptions.SetBitmapScalingMode(this, BitmapScalingMode.Fant);
            }
            else
            {
                RenderOptions.SetEdgeMode(this, EdgeMode.Unspecified);
                RenderOptions.SetBitmapScalingMode(this, BitmapScalingMode.NearestNeighbor);
                //RenderOptions.ProcessRenderMode = RenderMode.Default;
            }
        }
        public void ScaleAt(double scaleX, double scaleY, double centerX, double centerY)
        {
            Matrix m = this.Matrix;
            m.ScaleAt(scaleX, scaleY, centerX, centerY);
            this.Matrix = m;

            FireTransfromChanged();
        }
        public void FireTransfromChanged()
        {
            if (TransfromChanged != null)
            {
                TransfromChanged(this, null);
            }
        }

        public void SetIdentity()
        {
            MatrixTransform mt = this.MatrixTransform;
            Matrix m = this.MatrixTransform.Matrix;
            m.SetIdentity();
            mt.Matrix = m;

            FireTransfromChanged();
        }
        public void TranslateTo(double x, double y)
        {
            MatrixTransform mt = this.MatrixTransform;
            Matrix m = mt.Matrix;
            m.OffsetX = x;
            m.OffsetY = y;
            mt.Matrix = m;

            FireTransfromChanged();
        }
        public void Translate(double x, double y)
        {
            MatrixTransform mt = this.MatrixTransform;
            Matrix m = mt.Matrix;
            m.Translate(x, y);
            mt.Matrix = m;

            FireTransfromChanged();
        }
        public void Rotate(double angle, double x, double y)
        {
            MatrixTransform mt = this.MatrixTransform;
            Matrix m = mt.Matrix;
            m.RotateAt(angle, x, y);
            mt.Matrix = m;

            FireTransfromChanged();
        }
        public Point InverseTransform(Point pt)
        {
            return this.RenderTransform.Inverse.Transform(pt);
        }
    }
}