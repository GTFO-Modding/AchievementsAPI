using AchievementsAPI.Converters;
using System.Text.Json.Serialization;

namespace AchievementsAPI.Conditions
{
    [JsonConverter(typeof(ConditionOverridesConverter))]
    public class ConditionOverrides
    {
        public bool HasOverrides { get; set; }
        public AchievementConditionList AdditionalConditions { get; set; }

        public ConditionOverrides()
        {
            this.HasOverrides = false;
            this.AdditionalConditions = new();
        }
    }
}
