using AchievementsAPI.Managers;
using AchievementsAPI.Triggers;
using AchievementsAPI.Triggers.Registries;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace AchievementsAPI.Progress
{
    [JsonConverter(typeof(Converter))]
    internal sealed class AchievementProgress
    {
        public List<TriggerInfo> Triggers { get; set; } = new();

        public TriggerInfo GetTriggerInfo(string id, uint increment = 0)
        {
            if (this.Triggers == null)
            {
                this.Triggers = new();
                TriggerInfo info = new(id, increment);
                this.Triggers.Add(info);

                return info;
            }

            foreach (TriggerInfo info in this.Triggers)
            {
                if (info.ID == id && info.Increment == increment)
                {
                    return info;
                }
            }

            TriggerInfo newInfo = new(id, increment);
            this.Triggers.Add(newInfo);
            return newInfo;
        }

        public sealed class TriggerInfo
        {
            public string ID { get; set; }
            public uint Increment { get; set; }
            public IAchievementTriggerProgress Progress { get; set; }

            public TriggerInfo()
            {
                this.ID = null!;
                this.Progress = new AchievementTriggerProgress();
            }

            public TriggerInfo(string id, uint increment)
            {
                this.ID = id ?? throw new ArgumentNullException(nameof(id));
                this.Increment = increment;
                this.Progress = new AchievementTriggerProgress(); 
            }
            public TriggerInfo(string id, uint increment, IAchievementTriggerProgress progress) : this(id, increment)
            {
                this.Progress = progress;
            }
        }

        private sealed class Converter : JsonConverter<AchievementProgress>
        {
            public override AchievementProgress Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                AssertReaderToken(reader, JsonTokenType.StartObject);

                AchievementProgress info = new();
                string property = ReadProperty(ref reader);
                if (property.ToLower() != "triggers")
                {
                    throw new JsonException($"First property of AchievementProgress should be a Triggers field! Instead got {property}.");
                }


                AssertRead(ref reader);
                AssertReaderToken(reader, JsonTokenType.StartArray);

                while (reader.Read())
                {
                    JsonTokenType token = reader.TokenType;
                    AssertToken(token, JsonTokenType.StartObject, JsonTokenType.EndArray);

                    if (token == JsonTokenType.EndArray)
                    {
                        AssertRead(ref reader);
                        AssertReaderToken(reader, JsonTokenType.EndObject);

                        return info;
                    }

                    TriggerInfo? triggerInfo = ReadTriggerInfo(ref reader, options);
                    if (triggerInfo != null)
                    {
                        info.Triggers.Add(triggerInfo);
                    }
                }

                ThrowUnexpectedEOI();
                return info;
            }

            public override void Write(Utf8JsonWriter writer, AchievementProgress value, JsonSerializerOptions options)
            {
                writer.WriteStartObject();
                writer.WritePropertyName(nameof(value.Triggers));
                writer.WriteStartArray();
                foreach (TriggerInfo? trigger in value.Triggers)
                {
                    WriteTriggerInfo(writer, trigger, options);
                }
                writer.WriteEndArray();
                writer.WriteEndObject();
            }

            private static TriggerInfo ReadTriggerInfo(ref Utf8JsonReader reader, JsonSerializerOptions options)
            {
                AssertReaderToken(reader, "whilst reading start of TriggerInfo", JsonTokenType.StartObject);

                TriggerInfo info = new();
                string id = ReadID(ref reader);
                uint increment = ReadIncrement(ref reader);
                IAchievementTriggerProgress progress = ReadProgress(ref reader, id, options);

                info.ID = id;
                info.Increment = increment;
                info.Progress = progress;
                // todo: figure out why this errors out.
                //AssertRead(ref reader);
                //AssertReaderToken(reader, "whilst reading end of TriggerInfo", JsonTokenType.EndObject);

                return info;
            }

            private static string ReadProperty(ref Utf8JsonReader reader)
            {
                string property = ReadProperty(ref reader, out JsonTokenType readTokenType);
                AssertToken(readTokenType, "whilst reading property name", JsonTokenType.PropertyName);
                return property;
            }

            private static string ReadProperty(ref Utf8JsonReader reader, out JsonTokenType readTokenType)
            {
                AssertRead(ref reader);

                if (reader.TokenType != JsonTokenType.PropertyName)
                {
                    readTokenType = reader.TokenType;
                    return "";
                }
                readTokenType = JsonTokenType.PropertyName;
                return reader.GetString() ?? throw new JsonException("Property name null!");
            }

            private static string ReadID(ref Utf8JsonReader reader)
            {
                string property = ReadProperty(ref reader);   
                if (property.ToLower() != "id")
                {
                    throw new JsonException($"First property of TriggerInfo should be an ID field! Instead got {property}.");
                }

                AssertRead(ref reader);

                if (reader.TokenType == JsonTokenType.Null)
                {
                    throw new JsonException($"An ID cannot be 'null'");
                }

                AssertReaderToken(reader, "for ID", JsonTokenType.String);

                return reader.GetString()!;
            }

            private static uint ReadIncrement(ref Utf8JsonReader reader)
            {
                string property = ReadProperty(ref reader);
                if (property.ToLower() != "increment")
                {
                    throw new JsonException($"Second property of TriggerInfo should be an Increment field! Instead got {property}.");
                }

                AssertRead(ref reader);

                if (reader.TokenType == JsonTokenType.Null)
                {
                    throw new JsonException($"An Increment cannot be 'null'");
                }

                AssertReaderToken(reader, "for Increment", JsonTokenType.Number);

                return reader.GetUInt32();
            }

            private static IAchievementTriggerProgress ReadProgress(ref Utf8JsonReader reader, string id, JsonSerializerOptions options)
            {
                string property = ReadProperty(ref reader);
                if (property.ToLower() != "progress")
                {
                    throw new JsonException($"Third property of TriggerInfo should be a Progress field! Instead got {property}.");
                }

                AssertRead(ref reader);

                TriggerElementFactorySettings? triggerInfo = RegistryManager.Triggers[id];
                IAchievementTriggerProgress progress = triggerInfo.CreateProgressInstance();

                if (reader.TokenType == JsonTokenType.Null)
                {
                    return progress;
                }
                AssertReaderToken(reader, JsonTokenType.StartObject);

                progress.TriggerCount = ReadTriggerCount(ref reader);
                progress.Data = ReadData(ref reader, triggerInfo, options, out bool isEndObject);

                if (!isEndObject)
                {
                    AssertRead(ref reader);
                    if (!reader.Read())
                    {
                        ThrowUnexpectedEOI();
                    }

                    AssertReaderToken(reader, JsonTokenType.EndObject);
                }

                return progress;
            }

            private static int ReadTriggerCount(ref Utf8JsonReader reader)
            {
                string property = ReadProperty(ref reader);
                if (property.ToLower() != "triggercount")
                {
                    throw new JsonException($"First property of TriggerProgressData should be a TriggerCount field! Instead got {property}.");
                }

                AssertRead(ref reader);

                if (reader.TokenType == JsonTokenType.Null)
                {
                    throw new JsonException($"A TriggerCount cannot be 'null'");
                }

                AssertReaderToken(reader, "for TriggerCount", JsonTokenType.Number);
                return reader.GetInt32();
            }

            private static TriggerProgressData? ReadData(ref Utf8JsonReader reader, TriggerElementFactorySettings triggerInfo, JsonSerializerOptions options, out bool isEndObject)
            {
                isEndObject = false;
                if (triggerInfo.HasNoData)
                {
                    return null;
                }

                Type progressDataType = triggerInfo.ProgressDataType!;

                string property = ReadProperty(ref reader, out JsonTokenType readTokenType);
                if (readTokenType == JsonTokenType.EndObject)
                {
                    isEndObject = true;
                    return triggerInfo.CreateProgressDataInstance();
                }

                if (property.ToLower() != "data")
                {
                    throw new JsonException($"Third property of TriggerInfo should be a Data field! Instead got {property}.");
                }

                TriggerProgressData? data = (TriggerProgressData?)JsonSerializer.Deserialize(ref reader, progressDataType, options);
                if (data == null)
                {
                    data = triggerInfo.CreateProgressDataInstance();
                }
                return data;

            }

            private static void ThrowUnexpectedEOI()
                => throw new JsonException("Unexpected end of input!");

            private static void AssertRead(ref Utf8JsonReader reader)
            {
                if (!reader.Read())
                {
                    ThrowUnexpectedEOI();
                }
            }
            private static void AssertReaderToken(Utf8JsonReader reader, params JsonTokenType[] validTokens)
                => AssertToken(reader.TokenType, validTokens);
            private static void AssertReaderToken(Utf8JsonReader reader, string? context, params JsonTokenType[] validTokens)
                => AssertToken(reader.TokenType, context, validTokens);

            private static void AssertToken(JsonTokenType token, params JsonTokenType[] validTokens)
            {
                foreach (JsonTokenType tokenType in validTokens)
                {
                    if (token == tokenType)
                    {
                        return;
                    }
                }

                ThrowUnexpectedToken(token, validTokens);
            }
            private static void AssertToken(JsonTokenType token, string? context, params JsonTokenType[] validTokens)
            {
                foreach (JsonTokenType tokenType in validTokens)
                {
                    if (token == tokenType)
                    {
                        return;
                    }
                }

                ThrowUnexpectedToken(context, token, validTokens);
            }

            private static void ThrowUnexpectedToken(JsonTokenType token, params JsonTokenType[] validTokens)
            {
                ThrowUnexpectedToken(null, token, validTokens);
            }
            private static void ThrowUnexpectedToken(string? context, JsonTokenType token, params JsonTokenType[] validTokens)
            {
                StringBuilder messageBuilder = new();
                messageBuilder.Append("Unexpected token '");
                messageBuilder.Append(token);
                messageBuilder.Append('\'');
                if (!string.IsNullOrWhiteSpace(context))
                {
                    messageBuilder.Append(' ');
                    messageBuilder.Append(context);
                }

                if (messageBuilder[messageBuilder.Length - 1] != '.')
                {
                    messageBuilder.Append('.');
                }

                messageBuilder.Append(" (Expected ");
                if (validTokens.Length == 0)
                {
                    messageBuilder.Append("UnknownToken");
                }
                else
                {
                    bool twoOrMore = validTokens.Length >= 2;
                    bool threeOrMore = validTokens.Length >= 3;

                    for (int index = 0, length = validTokens.Length - 1; index < length; index++)
                    {
                        messageBuilder.Append(validTokens[index]);

                        if (threeOrMore)
                        {
                            messageBuilder.Append(',');
                        }
                    }

                    if (validTokens.Length > 1)
                    {
                        if (twoOrMore)
                        {
                            messageBuilder.Append(" or ");
                        }

                        messageBuilder.Append(validTokens[validTokens.Length - 1]);
                    }
                }
                messageBuilder.Append(')');




                throw new JsonException(messageBuilder.ToString());
            }

            private static void WriteTriggerInfo(Utf8JsonWriter writer, TriggerInfo value, JsonSerializerOptions options)
            {
                writer.WriteStartObject();
                writer.WriteString(nameof(value.ID), value.ID);
                writer.WriteNumber(nameof(value.Increment), value.Increment);
                TriggerElementFactorySettings? triggerInfo = RegistryManager.Triggers[value.ID];
                writer.WritePropertyName(nameof(value.Progress));
                writer.WriteStartObject();
                writer.WriteNumber(nameof(value.Progress.TriggerCount), value.Progress.TriggerCount);
                
                if (!triggerInfo.HasNoData)
                {
                    writer.WritePropertyName(nameof(value.Progress.Data));
                    JsonSerializer.Serialize(writer, value.Progress.Data, triggerInfo.ProgressDataType!, options);
                }
                writer.WriteEndObject();
                writer.WriteEndObject();
            }
        }
    }
}
