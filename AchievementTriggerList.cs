using AchievementsAPI.Converters;
using AchievementsAPI.Registries;
using AchievementsAPI.Triggers;
using System.Text.Json.Serialization;

namespace AchievementsAPI
{
    [JsonConverter(typeof(AchievementTriggerListConverter))]
    public sealed class AchievementTriggerList : RegistryList<IAchievementTrigger>
    {
        public AchievementTriggerList()
        { }

        public AchievementTriggerList(AchievementTriggerList other)
        {
            if (other == null)
                return;

            foreach (IAchievementTrigger trigger in other)
            {
                this.Add(trigger);
            }
        }
    }
}
