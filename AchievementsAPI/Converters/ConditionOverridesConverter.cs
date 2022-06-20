using AchievementsAPI.Conditions;
using AchievementsAPI.Conditions.Registries;
using AchievementsAPI.Utilities;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace AchievementsAPI.Converters
{
    /// <summary>
    /// A JsonConverter for handling <see cref="ConditionOverrides"/>.
    /// </summary>
    public class ConditionOverridesConverter : JsonConverter<ConditionOverrides>
    {
        /// <inheritdoc/>
        public override ConditionOverrides? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var overrides = new ConditionOverrides();

            if (reader.TokenType == JsonTokenType.Null)
            {
                return overrides;
            }

            if (reader.TokenType != JsonTokenType.StartObject)
            {
                throw new JsonException($"Unexpected token {reader.TokenType}. (Expected {JsonTokenType.StartObject})");
            }

            bool hasOverrides = false;
            bool hasAdditionalConditions = false;
            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndObject)
                {
                    return overrides;
                }

                if (reader.TokenType != JsonTokenType.PropertyName)
                {
                    throw new JsonException($"Unexpected token {reader.TokenType}. (Expected {JsonTokenType.PropertyName} or {JsonTokenType.EndObject})");
                }

                string propertyName = reader.GetString() ?? throw new JsonException("Null property name!");
                string loweredPropertyName = propertyName.ToLower();

                if (!reader.Read())
                {
                    goto UNEXPECTED_EOI;
                }

                if (loweredPropertyName == "hasoverrides")
                {
                    if (hasOverrides)
                    {
                        L.Warn($"Condition Overrides already has a value for HasOverrides! Duplicated property '{propertyName}'.");
                        JsonSerializer.Deserialize<bool>(ref reader, options);
                        continue;
                    }
                    hasOverrides = true;
                    overrides.HasOverrides = JsonSerializer.Deserialize<bool>(ref reader, options);
                }
                else if (loweredPropertyName == "additionalconditions")
                {
                    if (hasAdditionalConditions)
                    {
                        L.Warn($"Condition Overrides already has a value for AdditionalConditions! Duplicated property '{propertyName}'.");
                        JsonSerializer.Deserialize<AchievementConditionList>(ref reader, options);
                        continue;
                    }
                    hasAdditionalConditions = true;
                    overrides.AdditionalConditions = JsonSerializer.Deserialize<AchievementConditionList>(ref reader, options) ?? overrides.AdditionalConditions;
                }
                else
                {
                    throw new JsonException($"Unexpected property with name {propertyName}. Remove it to get rid of this error.");
                }
            }

            UNEXPECTED_EOI:
            throw new JsonException("Unexpected end of input.");

        }

        /// <inheritdoc/>
        public override void Write(Utf8JsonWriter writer, ConditionOverrides value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();

            writer.WriteBoolean(nameof(value.HasOverrides), value.HasOverrides);
            writer.WritePropertyName(nameof(value.AdditionalConditions));
            JsonSerializer.Serialize(writer, value.AdditionalConditions, options);
            writer.WriteEndObject();
        }
    }
}
