using DrawingBoard.Primitive;

namespace DrawingBoard.Serializables
{
    public class SerializableDot : SerializableBase
    {
        public double X
        {
            get { return _x; }
            set { _x = value; }
        }

        public double Y
        {
            get { return _y; }
            set { _y = value; }
        }

        public int Duration
        {
            get { return _duration; }
            set { _duration = value; }
        }


        private double _x;
        private double _y;
        private int _duration;

        public SerializableDot()
        {
            _x = 0;
            _y = 0;
            _duration = 0;
        }

        public SerializableDot(Dot dot)
        {
            _x = dot.X;
            _y = dot.Y;
            _duration = dot.Duration;
        }

        public override PrimitiveBase CreatePrimitive()
        {
            Dot circle = new Dot();

            circle.X = _x;
            circle.Y = _y;
            circle.Duration = _duration;

            return circle;
        }
    }
}
