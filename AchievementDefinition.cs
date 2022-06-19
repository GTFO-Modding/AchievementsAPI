using AchievementsAPI.Registries;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace AchievementsAPI
{
    [JsonConverter(typeof(Converter))]
    public class AchievementDefinition : IRegisterable
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string? Icon { get; set; }
        public bool IsSecret { get; set; }
        public int AchievementPoints { get; set; }
        public AchievementTriggerList Triggers { get; set; }
        public AchievementConditionList Conditions { get; set; }
        public string ID { get; set; }

        public AchievementDefinition()
        {
            this.Name = string.Empty;
            this.Description = string.Empty;
            this.Icon = string.Empty;
            this.IsSecret = false;
            this.AchievementPoints = 0;
            this.Triggers = new();
            this.Conditions = new();
            this.ID = string.Empty;
        }

        string IRegisterable.GetID() => this.ID;

        public sealed class Converter : JsonConverter<AchievementDefinition>
        {
            public override AchievementDefinition? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                var raw = JsonSerializer.Deserialize<Raw>(ref reader, options);
                if (raw == null)
                {
                    return null;
                }

                return raw.ToAchievement();
            }

            public override void Write(Utf8JsonWriter writer, AchievementDefinition value, JsonSerializerOptions options)
            {
                var raw = new Raw(value);
                JsonSerializer.Serialize(writer, raw, options);
            }
        }

        private sealed class Raw
        {
            public string? Name { get; set; }
            public string? Description { get; set; }
            public string? Icon { get; set; }
            public bool IsSecret { get; set; }
            public int AchievementPoints { get; set; }
            public AchievementTriggerList? Triggers { get; set; }
            public AchievementConditionList? Conditions { get; set; }
            public string? ID { get; set; }

            public Raw()
            { }

            public Raw(AchievementDefinition definition)
            {
                this.Name = definition.Name;
                this.Description = definition.Description;
                this.Icon = definition.Icon;
                this.IsSecret = definition.IsSecret;
                this.AchievementPoints = definition.AchievementPoints;
                this.Triggers = new(definition.Triggers);
                this.Conditions = new(definition.Conditions);
                this.ID = definition.ID;
            }

            public AchievementDefinition ToAchievement()
            {
                AchievementDefinition achievement = new()
                {
                    Name = this.Name ?? throw new Exception("A name is required for an achievement!"),
                    Description = this.Description ?? "No Description Provided",
                    Icon = this.Icon,
                    ID = this.ID ?? throw new Exception("An ID is required for an achievement!"),
                    IsSecret = this.IsSecret,
                    AchievementPoints = this.AchievementPoints,
                };
                if (this.Triggers != null)
                {
                    foreach (var trigger in this.Triggers)
                    {
                        achievement.Triggers.Add(trigger);
                    }
                }
                if (this.Conditions != null)
                {
                    foreach (var condition in this.Conditions)
                    {
                        achievement.Conditions.Add(condition);
                    }
                }
                return achievement;
            }
        }
    }
}
