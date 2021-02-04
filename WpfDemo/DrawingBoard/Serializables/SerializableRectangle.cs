using DrawingBoard.Primitive;

namespace DrawingBoard.Serializables
{
    public class SerializableRectangle : SerializableBase
    {
        private double _top;
        private double _left;
        private double _width;
        private double _height;

        public double Top
        {
            get { return _top; }
            set { _top = value; }
        }
        public double Left
        {
            get { return _left; }
            set { _left = value; }
        }
        public double Width
        {
            get { return _width; }
            set { _width = value; }
        }
        public double Height
        {
            get { return _height; }
            set { _height = value; }
        }

        public SerializableRectangle()
        {
            _top = 0;
            _left = 0;
            _width = 0;
            _height = 0;
        }
        public SerializableRectangle(Rectangle rect)
        {
            _top = rect.Top;
            _left = rect.Left;
            _width = rect.Width;
            _height = rect.Height;
        }

        public override PrimitiveBase CreatePrimitive()
        {
            Rectangle rect = new Rectangle();

            rect.Left = _left;
            rect.Top = _top;
            rect.Width = _width;
            rect.Height = _height;

            return rect;
        }
    }
}
