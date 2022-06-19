using Enemies;
using System.Collections.Generic;

namespace AchievementsAPI.Utilities
{
    /// <summary>
    /// Restrictions for specific enemies.
    /// </summary>
    public sealed class EnemyRestrictions
    {
        /// <summary>
        /// Whether or not to use the whitelist or blacklist.
        /// </summary>
        public bool UseWhiteList { get; set; }
        /// <summary>
        /// Enemies allowed.
        /// </summary>
        public List<uint>? WhiteList { get; set; }
        /// <summary>
        /// Enemies not allowed.
        /// </summary>
        public List<uint>? BlackList { get; set; }

        /// <summary>
        /// Returns whether or not the given enemy is valid by checking if it's in
        /// the whitelist or blacklist with it's enemy data id.
        /// </summary>
        /// <param name="enemy">The enemy to check.</param>
        /// <returns><see langword="true"/> if the enemy is valid, otherwise
        /// <see langword="false"/></returns>
        public bool IsValid(EnemyAgent enemy)
        {
            if (this.UseWhiteList)
            {
                return this.WhiteList?.Contains(enemy.EnemyDataID) ?? false;
            }
            else
            {
                return !(this.BlackList?.Contains(enemy.EnemyDataID) ?? false);
            }
        }
    }
}
