using AchievementsAPI.Conditions.Registries;
using AchievementsAPI.Registries;
using AchievementsAPI.Triggers.Registries;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace AchievementsAPI.Achievements
{
    /// <summary>
    /// A representation of an achievement.
    /// </summary>
    [JsonConverter(typeof(Converter))]
    public class AchievementDefinition : IRegisterable
    {
        /// <summary>
        /// The name of the achievement.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// The achievement's description.
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// The icon of the achievement. Not currently used, but will
        /// be used when UI of achievements gets added.
        /// </summary>
        public string? Icon { get; set; }
        /// <summary>
        /// The sound effect for completing the achievement.
        /// </summary>
        public AchievementSound CompletionSound { get; set; }
        /// <summary>
        /// Whether or not this achievement is a secret achievement, meaning
        /// it's name, description, icon, and achievement points are only revealed
        /// after unlocking it.
        /// </summary>
        public bool IsSecret { get; set; }
        /// <summary>
        /// The amount of achievement points this achievement is worth.
        /// </summary>
        public int AchievementPoints { get; set; }
        /// <summary>
        /// The list of triggers that trigger this achievement.
        /// </summary>
        public AchievementTriggerList Triggers { get; set; }
        /// <summary>
        /// The list of conditions required for each trigger.
        /// </summary>
        public AchievementConditionList Conditions { get; set; }
        /// <summary>
        /// The Unique ID of this trigger.
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// Creates a new instance of an achievement, setting everything
        /// to <see langword="default"/> or <see cref="string.Empty"/>.
        /// <para>
        /// <see cref="CompletionSound"/>, <see cref="Triggers"/>, and
        /// <see cref="Conditions"/> are initialized to a new instance.
        /// </para>
        /// </summary>
        public AchievementDefinition()
        {
            this.Name = string.Empty;
            this.Description = string.Empty;
            this.Icon = string.Empty;
            this.CompletionSound = new();
            this.IsSecret = false;
            this.AchievementPoints = 0;
            this.Triggers = new();
            this.Conditions = new();
            this.ID = string.Empty;
        }

        string IRegisterable.GetID() => this.ID;

        private sealed class Converter : JsonConverter<AchievementDefinition>
        {
            public override AchievementDefinition? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                Raw? raw = JsonSerializer.Deserialize<Raw>(ref reader, options);
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
            public AchievementSound? CompletionSound { get; set; }
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
                this.CompletionSound = new()
                {
                    Enabled = definition.CompletionSound.Enabled,
                    SoundID = definition.CompletionSound.SoundID,
                };
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
                    CompletionSound = new AchievementSound()
                    {
                        Enabled = this.CompletionSound?.Enabled ?? false,
                        SoundID = this.CompletionSound?.SoundID ?? 0U,
                    },
                    ID = this.ID ?? throw new Exception("An ID is required for an achievement!"),
                    IsSecret = this.IsSecret,
                    AchievementPoints = this.AchievementPoints,
                };
                if (this.Triggers != null)
                {
                    foreach (Triggers.IAchievementTriggerBase? trigger in this.Triggers)
                    {
                        achievement.Triggers.Add(trigger);
                    }
                }
                if (this.Conditions != null)
                {
                    foreach (Conditions.IAchievementCondition? condition in this.Conditions)
                    {
                        achievement.Conditions.Add(condition);
                    }
                }
                return achievement;
            }
        }
    }
}
