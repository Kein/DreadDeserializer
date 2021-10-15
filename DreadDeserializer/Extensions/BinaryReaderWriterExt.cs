using System;
using System.IO;
using System.Text;

namespace DreadDeserializer.Extensions
{
    public static class BinaryReaderWriterExt
    {

        //
        // READER
        //
        
        public static string ReadCString(this BinaryReader reader)
        {
            string result = null;
            var pos = reader.BaseStream.Position;
            byte c = reader.ReadByte();
            while (c != (byte)0)
                c = reader.ReadByte();
            var len = (int)(reader.BaseStream.Position - pos);
            reader.BaseStream.Position = pos;
            Span<byte> bytes = reader.ReadBytes(len);
            result = Encoding.ASCII.GetString(bytes[..^1]);
            return result;
        }

        //
        // WRITER
        //

        public static void WriteCString(this BinaryWriter writer, string str)
        {
            if (string.IsNullOrEmpty(str))
                throw new InvalidDataException("String has no value");
            Span<byte> bytes = Encoding.ASCII.GetBytes(str);
            writer.Write(bytes);
            writer.Write((byte)0);
        }

        public static void WriteObject(this BinaryWriter writer, object val)
        {
            var type = val.GetType();
            switch (val)
            {
                case byte bval:
                    writer.Write(bval);
                    break;
                case sbyte sbval:
                    writer.Write(sbval);
                    break;
                case int ival:
                    writer.Write(ival);
                    break;
                case uint uival:
                    writer.Write(uival);
                    break;
                case long lval:
                    writer.Write(lval);
                    break;
                case ulong ulval:
                    writer.Write(ulval);
                    break;
                case string strval:
                    writer.WriteCString(strval);
                    break;
                case float fval:
                    writer.Write(fval);
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        //
        // AUXULARY
        //

        public static int Seek(this BinaryReader reader, int count)
        {
            reader.BaseStream.Position = reader.BaseStream.Position + count;
            return (int)reader.BaseStream.Position;
        }

    }
}
