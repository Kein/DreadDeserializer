using DreadDeserializer.Extensions;
using System.Collections.Generic;
using System.IO;

namespace DreadDeserializer.Model
{
    public class Property : PropertyBase
    {
        private int State { get; set; }
        private ulong ArrayType  { get; set; }
        private int Count  { get; set; }
        public Dictionary<string, object> Data;

        public new void Deserialize(BinaryReader reader)
        {
            base.Deserialize(reader);
            State = reader.ReadInt32();
            ArrayType = reader.ReadUInt64();
            Count = reader.ReadInt32();
            if (Count > 0)
            {
                Data = new Dictionary<string, object>(Count);
                for (int i = 0; i < Count; i++)
                {
                    var pos = reader.BaseStream.Position;
                    var key = reader.ReadCString();
                    var type = reader.ReadUInt64();
                    reader.BaseStream.Position = pos;
                    object value = BDeserializer.ReadDreadType(reader, type);
                    Data[key] = value;
                }
            }
        }

        public new void Serialize(BinaryWriter writer)
        {
            throw new System.NotImplementedException();
        }
    }
}
