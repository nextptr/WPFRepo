using DrawingBoard.Primitive;

namespace DrawingBoard.Serializables
{
    public class SerializableCircle : SerializableBase
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

        public double Radius
        {
            get { return _radius; }
            set { _radius = value; }
        }

        private double _x;
        private double _y;
        private double _radius;

        public SerializableCircle()
        {
            _x = 0;
            _y = 0;
            _radius = 0;
        }

        public SerializableCircle(Circle circle)
        {
            _x = circle.X;
            _y = circle.Y;
            _radius = circle.Radius;
        }

        public override PrimitiveBase CreatePrimitive()
        {
            Circle circle = new Circle();

            circle.X = _x;
            circle.Y = _y;
            circle.Radius = _radius;

            return circle;
        }
    }
}
