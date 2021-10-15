using DreadDeserializer.Model;
using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace DreadDeserializer
{
    public class Program
    {
        static readonly byte[] header = new byte[] { 0xD0, 0xBE, 0x2F, 0x66, 0x27, 0x8B, 0xC8, 0x19, 0x03, 0x00, 0x01, 0x00, 0xCB, 0xC5, 0xEA, 0x0B,
                                                     0xB5, 0x42, 0x66, 0x34, 0x02, 0x00, 0x00, 0x00, 0x22, 0xB1, 0x6B, 0x43, 0x6D, 0x9E, 0xC8, 0x0D };
        static readonly byte[] ending = new byte[] { 0x48, 0xC4, 0x21, 0x8C, 0x5F, 0x99, 0x59, 0x70, 0x00, 0x00, 0x00, 0x00 };
        private const string helpMsg =
            "\nUSAGE:\n" +
            "DreadDeserializer file.bmssv\n" +
            "(samus.bmssv currently is not supported)\n";
        
        public static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.Write(helpMsg);
                return;
            }
            var fpath = args[0].Trim();
            if (!Path.IsPathRooted(fpath))
                fpath = Path.GetFullPath(fpath);
            if (!File.Exists(fpath))
                throw new ArgumentException($"Invalid filepath specified:\n{fpath}");
            FileInfo info = new FileInfo(fpath);
            if (info.Length > 500000 || info.Length < 582)
                throw new ArgumentException($"Invalid file size detected:\n{fpath} ({info.Length})");

            byte[] data = File.ReadAllBytes(fpath);
            var SaveData = new List<Property>();

            using (MemoryStream mstream = new MemoryStream(data, false))
            {
                using (BinaryReader reader = new BinaryReader(mstream))
                {
                    Span<byte> head = reader.ReadBytes(32);
                    if (!head.SequenceEqual<byte>(header.AsSpan()))
                        throw new InvalidDataException($"File header does not math:\n{fpath})");
                    var count = reader.ReadInt32();
                    for (int i = 0; i < count; i++)
                    {
                        var propObj = new Property();
                        propObj.Deserialize(reader);
                        SaveData.Add(propObj);
                        if (BDeserializer.IsEndOfFile(reader))
                            break;
                    }
                }
            }
            var jsonSettings = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore, Formatting = Formatting.Indented };
            string jsonString = JsonConvert.SerializeObject(SaveData, jsonSettings);
            var newPath = Path.ChangeExtension(fpath, "json");
            File.WriteAllText(newPath, jsonString);
            Console.WriteLine("Looks done to me");
        }

        public static void Serialize(ISerializeable prop)
        {
            byte[] res = null;
            using (MemoryStream mstream = new MemoryStream())
            {
                using (BinaryWriter writer = new BinaryWriter(mstream))
                {
                    writer.Write(header);
                    writer.Write(1);
                    prop.Serialize(writer);
                    writer.Write(ending);
                }
                res = mstream.ToArray();
            }
            if (res != null)
                File.WriteAllBytes(@"N:\1.bin", res);
        }

    }
}
