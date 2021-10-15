using System;
using System.IO;

namespace DreadDeserializer.Model.Sub
{
    public class MinimapAreaBox : PropertyBase, ISerializeable
    {
        private new string Key { get; set; }
        private int State { get; set; }
        private ulong Type1 { get; set; }
        public Vector2D Position { get; set; } = new Vector2D();
        private ulong Type2 { get; set; }
        public Vector2D Size { get; set; }  = new Vector2D();

        public new void Deserialize(BinaryReader reader)
        {
            base.Deserialize(reader);
            State = reader.ReadInt32();
            Type1 = reader.ReadUInt64();
            Position.Deserialize(reader);
            Type2 = reader.ReadUInt64();
            Size.Deserialize(reader);
        }

        public new void Serialize(BinaryWriter writer)
        {
            throw new NotImplementedException();
        }
    }
}
