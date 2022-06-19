using AchievementsAPI.Conditions;
using AchievementsAPI.Registries;
using AchievementsAPI.Triggers;
using AchievementsAPI.Utilities;
using System.Text.Json;

namespace AchievementsAPI.Converters
{
    public class AchievementTriggerListConverter : InternalRegistryListConverter<AchievementTriggerList, IAchievementTrigger>
    {
        protected override void FillElement(IAchievementTrigger element, ref Utf8JsonReader reader, JsonSerializerOptions options)
        {
            bool hasData = false;
            bool hasCount = false;
            bool hasConditionOverrides = false;
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

                string loweredProperty = property.ToLower();
                if (loweredProperty == "data")
                {
                    if (hasData)
                    {
                        L.Warn($"Achievement Trigger already has data! Duplicated data property '{property}'.");
                        JsonSerializer.Deserialize(ref reader, element.GetDataType(), options);
                        continue;
                    }

                    element.Data = (TriggerData?)JsonSerializer.Deserialize(ref reader, element.GetDataType(), options)!;
                    hasData = true;
                }
                else if (loweredProperty == "count")
                {
                    if (hasCount)
                    {
                        L.Warn($"Achievement Trigger already has a count! Duplicated count property '{property}'.");
                        JsonSerializer.Deserialize<int>(ref reader, options);
                        continue;
                    }
                    element.Count = JsonSerializer.Deserialize<int>(ref reader, options);
                    hasCount = true;
                }
                else if (loweredProperty == "conditionoverrides")
                {
                    if (hasConditionOverrides)
                    {
                        L.Warn($"Achievement Trigger already has condition overrides! Duplicated condition overrides property '{property}'.");
                        JsonSerializer.Deserialize<ConditionOverrides>(ref reader, options);
                        continue;
                    }
                    element.ConditionOverrides = JsonSerializer.Deserialize<ConditionOverrides>(ref reader, options) ?? element.ConditionOverrides;
                    hasConditionOverrides = true;
                }
                else
                {
                    throw new JsonException($"Unexpected property with name {property}. Remove it to get rid of this error.");
                }


            }

        UNEXPECTED_EOI:
            throw new JsonException("Unexpected end of input");
        }

        protected override void WriteElementProperties(Utf8JsonWriter writer, IAchievementTrigger element, JsonSerializerOptions options)
        {
            writer.WritePropertyName("Count");
            writer.WriteNumberValue(element.Count);
            writer.WritePropertyName("Data");
            JsonSerializer.Serialize(writer, element.Data, element.GetDataType(), options);
        }

        protected override IAchievementTrigger CreateElementFromID(string id)
        {
            return AchievementManager.Registry.Triggers.CreateElement(id);
        }
    }
}
