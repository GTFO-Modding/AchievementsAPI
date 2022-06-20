using AchievementsAPI.Registries;
using AchievementsAPI.Triggers;
using System;

namespace AchievementsAPI.Conditions
{
    /// <summary>
    /// Represents a condition for an achievement.
    /// </summary>
    public interface IAchievementCondition : IRegisterable
    {
        /// <summary>
        /// The data of this condition.
        /// </summary>
        ConditionData Data { get; set; }

        /// <summary>
        /// Returns the type of the Data for this. condition.
        /// </summary>
        /// <returns>The type of the Data for this condition.</returns>
        Type GetDataType();

        /// <summary>
        /// Called to setup this condition.
        /// </summary>
        void Setup();
        /// <summary>
        /// Returns whether or not this condition is met.
        /// </summary>
        /// <returns>Whether or not this condition is met.</returns>
        bool IsMet();
        /// <summary>
        /// Returns whether or not this condition is met for the specific trigger.
        /// </summary>
        /// <param name="trigger">The trigger to test.</param>
        /// <returns>Whether or not this condition is met for the specific trigger.</returns>
        bool IsMetForTrigger(IAchievementTriggerBase trigger);
        /// <summary>
        /// Called whenever a trigger is activated.
        /// </summary>
        /// <param name="trigger">The trigger that was activated.</param>
        void OnTriggerActivated(IAchievementTriggerBase trigger);
    }
}
