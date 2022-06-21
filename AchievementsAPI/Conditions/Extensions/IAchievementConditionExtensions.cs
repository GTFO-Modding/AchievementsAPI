using AchievementsAPI.Triggers;
using AchievementsAPI.Utilities;
using System;

namespace AchievementsAPI.Conditions.Extensions
{
    /// <summary>
    /// Some extension methods for <see cref="IAchievementCondition"/>
    /// </summary>
    public static class IAchievementConditionExtensions
    {
        /// <summary>
        /// Calls <see cref="IAchievementCondition.IsMet"/>, but with a try/catch wrapper.
        /// </summary>
        /// <param name="condition">The condition to call <c>IsMet</c> on.</param>
        /// <param name="errorResult">The result to return if an exception is thrown.</param>
        /// <returns><see langword="true"/> if the condition is met, otherwise
        /// <see langword="false"/></returns>
        public static bool IsMetSafe(this IAchievementCondition condition, bool errorResult = true)
        {
            try
            {
                return condition.IsMet();
            }
            catch (Exception ex)
            {
                L.Error("Error whilst checking if condition is met: " + ex);
                return errorResult;
            }
        }
        /// <summary>
        /// Calls <see cref="IAchievementCondition.IsMetForTrigger"/>, but with a try/catch wrapper.
        /// </summary>
        /// <param name="condition">The condition to call <c>IsMetForTrigger</c> on.</param>
        /// <param name="achievementID">The ID of the achievement the trigger belongs to.</param>
        /// <param name="trigger">The trigger to validate.</param>
        /// <param name="triggerIncrement">The increment of the trigger in the achievement.</param>
        /// <param name="errorResult">The result to return if an exception is thrown.</param>
        /// <returns><see langword="true"/> if the condition is met, otherwise
        /// <see langword="false"/></returns>
        public static bool IsMetForTriggerSafe(this IAchievementCondition condition, string achievementID, IAchievementTriggerBase trigger, uint triggerIncrement, bool errorResult = true)
        {
            try
            {
                return condition.IsMetForTrigger(achievementID, trigger, triggerIncrement);
            }
            catch (Exception ex)
            {
                L.Error("Error whilst checking if condition is met for trigger: " + ex);
                return errorResult;
            }
        }

        internal static void InvokeBeforeTriggerActivated(this IAchievementCondition condition, string achievementID, IAchievementTriggerBase trigger, uint triggerIncrement)
        {
            try
            {
                condition.BeforeTriggerActivated(achievementID, trigger, triggerIncrement);
            }
            catch (Exception ex)
            {
                L.Error("Error whilst running 'BeforeTriggerActivated' callback: " + ex);
            }
        }

        internal static void InvokeAfterTriggerActivated(this IAchievementCondition condition, string achievementID, IAchievementTriggerBase trigger, uint triggerIncrement)
        {
            try
            {
                condition.AfterTriggerActivated(achievementID, trigger, triggerIncrement);
            }
            catch (Exception ex)
            {
                L.Error("Error whilst running 'AfterTriggerActivated' callback: " + ex);
            }
        }
    }
}
