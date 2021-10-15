using DreadDeserializer.Extensions;
using System;
using System.Collections.Generic;
using System.IO;

namespace DreadDeserializer.Model.Sub
{
    public class MissionLogListEntry : MissionLogProperty, ISerializeable
    {
        public new List<string> Entries { get; set; }

        public new void Deserialize(BinaryReader reader)
        {
            base.Deserialize(reader);
            if (Count > 0)
            {
                Entries = new List<string>(Count);
                for (int i = 0; i < Count; i++)
                    Entries.Add(reader.ReadCString());
            }
            State1 = reader.ReadInt32();
            if (State1 != 0x00000003)
            {
                reader.Seek(-4);
                return;
            }
            Type1 = reader.ReadUInt64();
            State2 = reader.ReadInt32();
            Type2 = reader.ReadUInt64();
        }

        public new void Serialize(BinaryWriter writer)
        {
            throw new NotImplementedException();
        }
    }
}
