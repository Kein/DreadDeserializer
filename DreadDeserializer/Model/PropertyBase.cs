using DreadDeserializer.Extensions;
using System;
using System.IO;

namespace DreadDeserializer.Model
{
    public class PropertyBase : ISerializeable
    {
        public string Key { get; set; }
        protected ulong Type { get; set; }

        public void Deserialize(BinaryReader reader)
        {
            Key = reader.ReadCString();
            Type = reader.ReadUInt64();
        }

        public void Serialize(BinaryWriter writer)
        {
            throw new NotImplementedException();
        }
    }
}
