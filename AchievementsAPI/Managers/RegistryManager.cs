using AchievementsAPI.Achievements.Registries;
using AchievementsAPI.Conditions.Registries;
using AchievementsAPI.Triggers.Registries;

namespace AchievementsAPI.Managers
{
    /// <summary>
    /// A manager for all possible registerable items.
    /// </summary>
    public static class RegistryManager
    {
        /// <summary>
        /// A registry for conditions.
        /// </summary>
        public static ConditionRegistry Conditions { get; } = new();
        /// <summary>
        /// A registry for triggers.
        /// </summary>
        public static TriggerRegistry Triggers { get; } = new();
        /// <summary>
        /// A registry for achievements.
        /// </summary>
        public static AchievementRegistry Achievements { get; } = new();
    }
}
