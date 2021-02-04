using DrawingBoard.Primitive;

namespace DrawingBoard.Serializables
{
    public class SerializableLine : SerializableBase
    {
        public double X1
        {
            get { return _x1; }
            set { _x1 = value; }
        }

        public double Y1
        {
            get { return _y1; }
            set { _y1 = value; }
        }

        public double X2
        {
            get { return _x2; }
            set { _x2 = value; }
        }

        public double Y2
        {
            get { return _y2; }
            set { _y2 = value; }
        }

        private double _x1;
        private double _y1;
        private double _x2;
        private double _y2;

        public SerializableLine()
        {
            _x1 = 0;
            _y1 = 0;
            _x2 = 0;
            _y2 = 0;
        }

        public SerializableLine(Line line)
        {
            _x1 = line.X1;
            _y1 = line.Y1;
            _x2 = line.X2;
            _y2 = line.Y2;
        }

        public override PrimitiveBase CreatePrimitive()
        {
            Line line = new Line();

            line.X1 = _x1;
            line.Y1 = _y1;
            line.X2 = _x2;
            line.Y2 = _y2;

            return line;
        }
    }
}
