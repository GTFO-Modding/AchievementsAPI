using AchievementsAPI.Conditions;
using AchievementsAPI.Conditions.Extensions;
using AchievementsAPI.Progress;
using AchievementsAPI.Utilities;
using System;

namespace AchievementsAPI.Triggers.Extensions
{
    /// <summary>
    /// Some extension methods for <see cref="IAchievementTriggerBase"/>
    /// </summary>
    public static class IAchievementTriggerExtensions
    {
        /// <summary>
        /// Returns whether or not the trigger meets all internally defined conditions.
        /// </summary>
        /// <param name="trigger">The trigger to check.</param>
        /// <param name="achievementID">The achievement the trigger belongs to.</param>
        /// <param name="triggerIncrement">The increment of the trigger in the achievement.</param>
        /// <param name="errorResult">The result to return for a condition
        /// if an exception is thrown.</param>
        /// <returns><see langword="true"/> if the trigger does meet all conditions,
        /// otherwise <see langword="false"/>.</returns>
        public static bool MeetsConditions(this IAchievementTriggerBase trigger, string achievementID, uint triggerIncrement, bool errorResult = true)
        {
            if (trigger == null)
            {
                throw new ArgumentNullException(nameof(trigger));
            }

            ConditionOverrides overrides = trigger.ConditionOverrides;
            if (overrides == null)
            {
                return true;
            }

            foreach (IAchievementCondition condition in overrides.AdditionalConditions)
            {
                if (!(condition.IsMetSafe(errorResult) && condition.IsMetForTriggerSafe(achievementID, trigger, triggerIncrement, errorResult)))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Returns whether this trigger can be triggered with
        /// the given data, wrapping the call to
        /// <see cref="IAchievementTriggerBase.CanBeTriggered(object?[])"/>
        /// in a Try Catch.
        /// <para>
        /// If this method returns <see langword="true"/>, then
        /// the <c>Trigger</c> method will be called.
        /// </para>
        /// </summary>
        /// <param name="trigger">The trigger to check.</param>
        /// <param name="data">The data for activating the trigger.</param>
        /// <param name="errorResult">The result to return if an exception is thrown.</param>
        /// <returns><see langword="true"/> if this trigger can be triggered
        /// with <paramref name="data"/>, otherwise <see langword="false"/>.</returns>
        public static bool CanBeTriggeredSafe(this IAchievementTriggerBase trigger, object?[] data, bool errorResult = true)
        {
            try
            {
                return trigger.CanBeTriggered(data);
            }
            catch (Exception ex)
            {
                L.Error("Error whilst checking if trigger can be activated: " + ex);
                return errorResult;
            }
        }

        internal static void TriggerSafe(this _IAchievementTriggerBase trigger, object?[] data, IAchievementTriggerProgress progress)
        {
            try
            {
                trigger.Trigger(data, progress);
            }
            catch (Exception ex)
            {
                L.Error("Error whilst activating trigger: " + ex);
            }
        }

        internal static void ResetProgressSafe(this _IAchievementTriggerBase trigger, IAchievementTriggerProgress progress)
        {
            try
            {
                trigger.ResetProgress(progress);
            }
            catch (Exception ex)
            {
                L.Error("Error whilst resetting progress for trigger: " + ex);
            }
        }
    }
}
