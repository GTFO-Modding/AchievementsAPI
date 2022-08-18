using AchievementsAPI.Triggers;
using Flaff.Collections.Registries;
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
        /// <param name="achievementID">The ID of the achievement the trigger belongs to.</param>
        /// <param name="trigger">The trigger to test.</param>
        /// <param name="triggerIncrement">The increment of the trigger in the achievement.</param>
        /// <returns>Whether or not this condition is met for the specific trigger.</returns>
        bool IsMetForTrigger(string achievementID, IAchievementTriggerBase trigger, uint triggerIncrement);
        /// <summary>
        /// Called before a trigger is activated.
        /// </summary>
        /// <param name="achievementID">The ID of the achievement the trigger belongs to.</param>
        /// <param name="trigger">The trigger that was activated.</param>
        /// <param name="triggerIncrement">The increment of the trigger in the achievement.</param>
        void BeforeTriggerActivated(string achievementID, IAchievementTriggerBase trigger, uint triggerIncrement);
        /// <summary>
        /// Called after a trigger is activated.
        /// </summary>
        /// <param name="achievementID">The ID of the achievement the trigger belongs to.</param>
        /// <param name="trigger">The trigger that was activated.</param>
        /// <param name="triggerIncrement">The increment of the trigger in the achievement.</param>
        void AfterTriggerActivated(string achievementID, IAchievementTriggerBase trigger, uint triggerIncrement);
    }
}
