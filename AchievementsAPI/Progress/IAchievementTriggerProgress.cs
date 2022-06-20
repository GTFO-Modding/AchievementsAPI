using AchievementsAPI.Triggers;
using System;

namespace AchievementsAPI.Progress
{
    internal interface IAchievementTriggerProgress : IEquatable<IAchievementTriggerProgress>
    {
        /// <summary>
        /// The times this trigger has been triggered.
        /// </summary>
        int TriggerCount { get; set; }

        TriggerProgressData? Data { get; set; }

        IAchievementTriggerProgress Clone();
    }
}
