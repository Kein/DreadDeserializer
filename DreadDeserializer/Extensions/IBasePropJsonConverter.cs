using DreadDeserializer.Model;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DreadDeserializer.Extensions
{
    class IBasePropJsonConverter : JsonConverter<ISerializeable>
    {
        public override bool CanConvert(Type type)
        {
            return typeof(ISerializeable).IsAssignableFrom(type);
        }

        public override ISerializeable Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }

        public override void Write(Utf8JsonWriter writer, ISerializeable value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}
