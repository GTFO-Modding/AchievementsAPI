using AchievementsAPI.Conditions;
using AchievementsAPI.Registries;
using AchievementsAPI.Utilities;
using System.Text.Json;

namespace AchievementsAPI.Converters
{
    public class AchievementConditionListConverter : InternalRegistryListConverter<AchievementConditionList, IAchievementCondition>
    {
        protected override void FillElement(IAchievementCondition element, ref Utf8JsonReader reader, JsonSerializerOptions options)
        {
            bool hasData = false;
            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndObject)
                {
                    return;
                }

                if (reader.TokenType != JsonTokenType.PropertyName)
                {
                    throw new JsonException($"Unexpected token type {reader.TokenType} (Expected {JsonTokenType.PropertyName})");
                }

                string property = reader.GetString() ?? throw new JsonException($"Property Name was null!");

                if (!reader.Read())
                    goto UNEXPECTED_EOI;

                if (property.ToLower() == "data")
                {
                    if (hasData)
                    {
                        L.Warn($"Achievement Condition already has data! Duplicated data property '{property}'.");
                        JsonSerializer.Deserialize(ref reader, element.GetDataType(), options);
                        continue;
                    }

                    element.Data = (ConditionData?)JsonSerializer.Deserialize(ref reader, element.GetDataType(), options)!;
                    hasData = true;
                }
                else
                {
                    throw new JsonException($"Unexpected property with name {property}. Remove it to get rid of this error.");
                }


            }

            UNEXPECTED_EOI:
            throw new JsonException("Unexpected end of input");
        }

        protected override void WriteElementProperties(Utf8JsonWriter writer, IAchievementCondition element, JsonSerializerOptions options)
        {
            writer.WritePropertyName("Data");
            JsonSerializer.Serialize(writer, element.Data, element.GetDataType(), options);
        }

        protected override IAchievementCondition CreateElementFromID(string id)
        {
            return AchievementManager.Registry.Conditions.CreateElement(id);
        }
    }
}
