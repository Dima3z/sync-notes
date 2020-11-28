using System;
using System.Text;
using Newtonsoft.Json;

namespace Notes.Core.JsonHelpers
{
    public class ContentConverter : JsonConverter<string>
    {
        public override void WriteJson(JsonWriter writer, string value, JsonSerializer serializer)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }

            var plainTextBytes = Encoding.UTF8.GetBytes(value);
            writer.WriteValue(Convert.ToBase64String(plainTextBytes));
        }

        public override string ReadJson(JsonReader reader, Type objectType, string existingValue, bool hasExistingValue,
            JsonSerializer serializer)
        {
            if (!(reader.Value is string value))
            {
                return null;
            }

            var bytes = Convert.FromBase64String(value);
            return Encoding.UTF8.GetString(bytes);
        }
    }
}