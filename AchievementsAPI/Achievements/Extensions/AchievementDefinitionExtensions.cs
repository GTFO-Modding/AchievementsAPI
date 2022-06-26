using AchievementsAPI.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AchievementsAPI.Achievements.Extensions
{
    /// <summary>
    /// Some extension methods for <see cref="AchievementDefinition"/>
    /// </summary>
    public static class AchievementDefinitionExtensions
    {
        /// <summary>
        /// Returns the progress of the achievement.
        /// </summary>
        /// <param name="achievement">The achievement to check.</param>
        /// <returns>A value between 0 and 1 [inclusive], where 0 means 0% completed, and
        /// 1 means 100% completed.</returns>
        public static double GetProgress(this AchievementDefinition achievement)
        {
            return AchievementManager.GetAchievementProgress(achievement.ID);
        }
        /// <summary>
        /// Returns whether or not the achievement is completed.
        /// </summary>
        /// <param name="achievement">The achievement to check.</param>
        /// <returns><see langword="true"/> if it is completed, otherwise
        /// <see langword="false"/>.</returns>
        public static bool IsCompleted(this AchievementDefinition achievement)
        {
            return AchievementManager.IsAchievementCompleted(achievement.ID);
        }
    }
}
