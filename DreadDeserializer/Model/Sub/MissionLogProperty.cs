using System.Collections.Generic;
using System.IO;

namespace DreadDeserializer.Model.Sub
{
    public class MissionLogProperty : PropertyBase, ISerializeable
    {
        protected int Count { get; set; }
        protected int? State1 { get; set; }
        protected ulong? Type1 { get; set; }
        protected int? State2 { get; set; }
        protected ulong? Type2 { get; set; }
        public List<object> Entries { get; set; }

        protected new void Deserialize(BinaryReader reader)
        {
            base.Deserialize(reader);
            Count = reader.ReadInt32();
        }
    }
}
