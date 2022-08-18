using AchievementsAPI.Converters;
using Flaff.Collections.Registries;
using System.Text.Json.Serialization;

namespace AchievementsAPI.Triggers.Registries
{
    /// <summary>
    /// A list of achievement triggers.
    /// </summary>
    [JsonConverter(typeof(AchievementTriggerListConverter))]
    public sealed class AchievementTriggerList : RegistryList<IAchievementTriggerBase>
    {
        /// <summary>
        /// Initializes this list as empty.
        /// </summary>
        public AchievementTriggerList()
        { }

        /// <summary>
        /// Initializes this list, copying all elements from <paramref name="other"/>
        /// into this list.
        /// </summary>
        /// <param name="other">The list to copy over.</param>
        public AchievementTriggerList(AchievementTriggerList other)
        {
            if (other == null)
            {
                return;
            }

            foreach (IAchievementTriggerBase trigger in other)
            {
                this.Add(trigger);
            }
        }
    }
}
