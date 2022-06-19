using AchievementsAPI.Conditions;
using AchievementsAPI.Converters;
using AchievementsAPI.Registries;
using System.Text.Json.Serialization;

namespace AchievementsAPI
{
    [JsonConverter(typeof(AchievementConditionListConverter))]
    public sealed class AchievementConditionList : RegistryList<IAchievementCondition>
    {
        public AchievementConditionList()
        { }

        public AchievementConditionList(AchievementConditionList other)
        {
            if (other == null)
                return;

            foreach (IAchievementCondition condition in other)
            {
                this.Add(condition);
            }
        }
    }
}
