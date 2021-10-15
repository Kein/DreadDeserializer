using System.IO;

namespace DreadDeserializer.Model.Sub
{
    public class Vector2D : ISerializeable
    {
        public float X;
        public float Y;

        public void Deserialize(BinaryReader reader)
        {
            X = reader.ReadSingle();
            Y = reader.ReadSingle();
        }

        public void Serialize(BinaryWriter writer)
        {
            writer.Write(X);
            writer.Write(Y);
        }
    }
}
