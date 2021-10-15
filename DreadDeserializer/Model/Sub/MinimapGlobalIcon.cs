using DreadDeserializer.Extensions;
using System;
using System.IO;

namespace DreadDeserializer.Model.Sub
{
    public class MinimapGlobalIcon : ISerializeable
    {
        public string IconName { get; set; }
        private ulong Type  { get; set; }
        public float X { get; set; }
        public float Y { get; set; }

        public void Serialize(BinaryWriter writer)
        {
            throw new NotImplementedException();
        }

        public void Deserialize(BinaryReader reader)
        {
            IconName = reader.ReadCString();
            Type = reader.ReadUInt64();
            X = reader.ReadSingle();
            Y = reader.ReadSingle();
            var next = reader.ReadInt32();
            if (next == 2)
                reader.Seek(8);
            else
                reader.Seek(-4);
        }
    }
}
