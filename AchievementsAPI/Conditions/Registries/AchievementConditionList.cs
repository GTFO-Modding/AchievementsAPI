using AchievementsAPI.Converters;
using System.Text.Json.Serialization;
using Flaff.Collections.Registries;

namespace AchievementsAPI.Conditions.Registries
{
    /// <summary>
    /// A list of conditions.
    /// </summary>
    [JsonConverter(typeof(AchievementConditionListConverter))]
    public sealed class AchievementConditionList : RegistryList<IAchievementCondition>
    {
        /// <summary>
        /// Initializes a new empty list of conditions.
        /// </summary>
        public AchievementConditionList()
        { }

        /// <summary>
        /// Initializes this list, copying all elements from <paramref name="other"/>
        /// into this list.
        /// </summary>
        /// <param name="other">The list to copy over.</param>
        public AchievementConditionList(AchievementConditionList other)
        {
            if (other == null)
            {
                return;
            }

            foreach (IAchievementCondition condition in other)
            {
                this.Add(condition);
            }
        }
    }
}
