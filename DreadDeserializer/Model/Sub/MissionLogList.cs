using System;
using System.Collections.Generic;
using System.IO;

namespace DreadDeserializer.Model.Sub
{
    public class MissionLogList : MissionLogProperty, ISerializeable
    {
        public new List<MissionLogListEntry> Entries { get; set; }

        public new void Deserialize(BinaryReader reader)
        {
            base.Deserialize(reader);
            State1 = reader.ReadInt32();
            Type1 = reader.ReadUInt64();
            State2 = reader.ReadInt32();
            Type2 = reader.ReadUInt64();
            if (Count > 0)
            {
                Entries = new List<MissionLogListEntry>(Count);
                for (int i = 0; i < Count; i++)
                {
                    var entry = new MissionLogListEntry();
                    entry.Deserialize(reader);
                    Entries.Add(entry);
                }
            }
        }

        public new void Serialize(BinaryWriter writer)
        {
            throw new NotImplementedException();
        }
    }
}
