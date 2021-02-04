using System.Xml.Serialization;
using DrawingBoard.Primitive;

namespace DrawingBoard.Serializables
{
    [XmlInclude(typeof(SerializableLine))]
    [XmlInclude(typeof(SerializableCircle))]
    [XmlInclude(typeof(SerializableRectangle))]
    public abstract class SerializableBase
    {
        public abstract PrimitiveBase CreatePrimitive();
    }
}
