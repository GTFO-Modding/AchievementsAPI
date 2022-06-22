using AchievementsAPI.Utilities;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace AchievementsAPI.Converters
{
    /// <summary>
    /// Json Converter for <see cref="DatablockReference"/>s.
    /// </summary>
    public sealed class DatablockReferenceConverter : JsonConverter<DatablockReference>
    {
        /// <inheritdoc/>
        public override DatablockReference? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return reader.TokenType switch
            {
                JsonTokenType.Null => null,
                JsonTokenType.String => new DatablockReference(reader.GetString()!),
                JsonTokenType.Number => new DatablockReference(reader.GetUInt32()),
                _ => throw new JsonException($"Unexpected token '{reader.TokenType}'")
            };
        }

        /// <inheritdoc/>
        public override void Write(Utf8JsonWriter writer, DatablockReference value, JsonSerializerOptions options)
        {
            if (value.Name == null)
            {
                writer.WriteNumberValue(value.ID);
            }
            else
            {
                writer.WriteStringValue(value.Name);
            }
        }
    }
}
