using DreadDeserializer.Extensions;
using System.Collections.Generic;
using System.IO;

namespace DreadDeserializer.Model.Sub
{
    public class MinimapGlobalIconsList : PropertyBase, ISerializeable
    {
        public int Count { get; set; }
        public Dictionary<string, MinimapGlobalIcon[]> Data;

        public new void Deserialize(BinaryReader reader)
        {
            base.Deserialize(reader);
            Count = reader.ReadInt32();
            if (Count > 0)
            {
                Data = new Dictionary<string, MinimapGlobalIcon[]>(Count);
                for (int i = 0; i < Count; i++)
                {
                    var key = reader.ReadCString();
                    var inCount = reader.ReadUInt32();
                    var next = reader.ReadUInt32();
                    if (next == 2)
                        reader.Seek(8);
                    else
                        reader.Seek(-4);
                    var value = new MinimapGlobalIcon[inCount];
                    for (int k = 0; k < inCount; k++)
                    {
                        var icon = new MinimapGlobalIcon();
                        icon.Deserialize(reader);
                        value[k] = icon;
                    }
                    Data[key] = value;
                }
            }

        }
    }
}
