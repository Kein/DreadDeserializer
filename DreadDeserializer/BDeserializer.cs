using DreadDeserializer.Extensions;
using DreadDeserializer.Model;
using DreadDeserializer.Model.Sub;
using System;
using System.Collections.Generic;
using System.IO;

namespace DreadDeserializer
{
    public static class BDeserializer
    {
        public delegate object PrimitiveReader(BinaryReader reader);

        public static Dictionary<Type, ulong> PropertyMap = new Dictionary<Type, ulong>()
        {
            { typeof(sbyte), (ulong)Constants.Byte },
            { typeof(byte), (ulong)Constants.Byte },
            { typeof(float), (ulong)Constants.Float },
            { typeof(string), (ulong)Constants.NameOrString }
        };

        public static Dictionary<Type, PrimitiveReader> PrimitiveMap = new Dictionary<Type, PrimitiveReader>()
        {
            { typeof(string), (r) => r.ReadCString() },
            { typeof(sbyte), (r) => r.ReadSByte() },
            { typeof(byte),  (r) => r.ReadByte() },
            { typeof(int), (r) => r.ReadInt32() },
            { typeof(float), (r) => r.ReadSingle() }
        };

        public static bool IsEndOfFile(BinaryReader reader)
        {
            bool res = reader.BaseStream.Length - reader.BaseStream.Position < 12;
            if (res == false)
            {
                var origPos = reader.BaseStream.Position;
                var end = reader.ReadUInt64();
                var end0 = reader.ReadInt32();
                if (end == (ulong)Constants.EOF && end0 == 0)
                    res = true;
                reader.BaseStream.Position = origPos;
            }
            return res;
        }

        public static void EnsureTypeMatch(BinaryReader reader, Constants against, bool rewind = false)
        {
            var pos = reader.BaseStream.Position;
            ulong newType = reader.ReadUInt64();
            if (rewind)
                reader.BaseStream.Position = pos;
            if (newType != (ulong)against)
                throw new InvalidDataException($"Type mismatch!\nExpected: {against.ToString()} pos: {pos}");
        }

        public static object ReadDreadType(BinaryReader reader, ulong type)
        {
            object result = null;
            switch (type)
            {
                case 0x6A0C7BDE338B1A2B: //byte
                    result = SkipPropertyBase(reader).ReadByte();
                    break;
                case 0x937459BA5ED68A51: //float
                    result = SkipPropertyBase(reader).ReadSingle();
                    break;
                case 0x799781F713E7D4E0: // literal string/name of the object? (used in LevelID)
                case 0xBF34F79BBA0DEAF6: // enum serialized as string? Used in Adam dialogue
                case 0x26DE4835F38BB831: // another one for ADAM, definitely enums?
                    result = SkipPropertyBase(reader).ReadCString();
                    break;
                case 0x291E3F130F064F1D: // integer2? (used in GAME_PROGRESS)
                case 0xD7278D3C2CC621ED: // int32 array indicator? (used to define array count in baseprops)
                case 0xB97CD894D76CACD6: // int32 real? (used in cutscenes)
                    result = SkipPropertyBase(reader).ReadInt32();
                    break;
                case 0xD2DCBE71A41CE64D: // MISSION_LOG:EventsList and TutosList collections
                    result = DeserializeComplex<MissionLogList>(reader);
                    break;
                case 0x55A2CCA8471AF58E: // entry for EventsList and TutosList
                    result = DeserializeComplex<MissionLogListEntry>(reader);
                    break;
                case 0xF9365AAB69F858F0: // entry for EventsList and TutosList
                    result = DeserializeComplex<MinimapGlobalIconsList>(reader);
                    break;
                case 0xF450E55A3654AABD: // entry for EventsList and TutosList
                    result = DeserializeComplex<MinimapAreaBox>(reader);
                    break;
                case 0xCF6AD2B17894E025: // array of string
                      result = ReadGenericArray<string>(SkipPropertyBase(reader));
                    break;
                case 0x29C6B98E3D947388: // array of bytes
                    result = ReadGenericArray<byte>(SkipPropertyBase(reader));
                    break;
                case 0x24873FF4B4E3C57E: // array of ints (MinimapTUTOS)
                    result = ReadGenericArray<int>(SkipPropertyBase(reader));
                    break;
                default:
                    throw new NotImplementedException();
            }
            return result;
        }

        public static BinaryReader SkipPropertyBase(BinaryReader reader)
        {
            reader.ReadCString();
            reader.Seek(8);
            return reader;
        }

        public static T DeserializeComplex<T>(BinaryReader reader)
        {
            ISerializeable propObj = Activator.CreateInstance<T>() as ISerializeable;
            propObj.Deserialize(reader);
            return (T)propObj;
        }

        public static T[] ReadGenericArray<T>(BinaryReader reader)
        {
            var count = reader.ReadUInt32();
            var r = PrimitiveMap[typeof(T)];
            T[] result = null;
            if (count > 0)
            {
                result = (T[])Array.CreateInstance(typeof(T), count);
                for (int i = 0; i < count; i++)
                    result[i] = (T)r(reader);
            }
            return result;
        }
    }
}
