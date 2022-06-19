namespace AchievementsAPI.Registries
{
    /// <summary>
    /// A handler for all possible registerable items.
    /// </summary>
    public sealed class RegistryHandler
    {
        /// <summary>
        /// A registry for conditions.
        /// </summary>
        public ConditionRegistry Conditions { get; } = new();
        /// <summary>
        /// A registry for triggers.
        /// </summary>
        public TriggerRegistry Triggers { get; } = new();
        /// <summary>
        /// A registry for achievements.
        /// </summary>
        public AchievementRegistry Achievements { get; } = new();
    }
}
