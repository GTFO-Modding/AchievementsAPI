using AchievementsAPI.Conditions.Registries;
using AchievementsAPI.Converters;
using System.Text.Json.Serialization;

namespace AchievementsAPI.Conditions
{
    /// <summary>
    /// Overrides for Conditions for Triggers. Currently allows for additional triggers.
    /// Eventually will allow for overriding conditions defined in the achievement.
    /// </summary>
    [JsonConverter(typeof(ConditionOverridesConverter))]
    public class ConditionOverrides
    {
        /// <summary>
        /// Whether or not this condition has overrides.
        /// </summary>
        public bool HasOverrides { get; set; }
        /// <summary>
        /// A list of additional conditions.
        /// </summary>
        public AchievementConditionList AdditionalConditions { get; set; }

        /// <summary>
        /// Initializes this overrides with no overrides and no additional
        /// conditions.
        /// </summary>
        public ConditionOverrides()
        {
            this.HasOverrides = false;
            this.AdditionalConditions = new();
        }
    }
}
