using System.IO;

namespace DreadDeserializer.Model
{
    public interface ISerializeable
    {
        public abstract void Serialize(BinaryWriter writer);
        public abstract void Deserialize(BinaryReader reader);

    }
}
