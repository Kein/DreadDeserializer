using System.IO;

namespace DreadDeserializer.Model
{
    public class ValueProperty<T> : PropertyBase, ISerializeable 
    {
        public T Value { get; set; }

        public new void Deserialize(BinaryReader reader)
        {
            
        }
    }
}
