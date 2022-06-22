using Enemies;
using GameData;
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
        public List<DatablockReference?>? WhiteList { get; set; }
        /// <summary>
        /// Enemies not allowed.
        /// </summary>
        public List<DatablockReference?>? BlackList { get; set; }

        /// <summary>
        /// Returns whether or not the given enemy is valid by checking if it's in
        /// the whitelist or blacklist with it's enemy data block.
        /// </summary>
        /// <param name="enemy">The enemy to check.</param>
        /// <returns><see langword="true"/> if the enemy is valid, otherwise
        /// <see langword="false"/></returns>
        public bool IsValid(EnemyAgent enemy)
            => this.IsValid(enemy.EnemyData);

        /// <summary>
        /// Returns whether or not the given enemy datablock is valid by checking if it's in
        /// the whitelist or blacklist.
        /// </summary>
        /// <param name="enemyDB">The enemy DB to check.</param>
        /// <returns><see langword="true"/> if the enemy DB is valid, otherwise
        /// <see langword="false"/></returns>
        public bool IsValid(EnemyDataBlock enemyDB)
        {
            return this.IsValid(enemyDB.persistentID) ||
                this.IsValid(enemyDB.name);
        }

        /// <summary>
        /// Returns whether or not the given enemy ID is valid by checking if it's in
        /// the whitelist or blacklist with it's enemy data id.
        /// </summary>
        /// <param name="enemyID">The enemy ID to check.</param>
        /// <returns><see langword="true"/> if the enemy ID is valid, otherwise
        /// <see langword="false"/></returns>
        public bool IsValid(uint enemyID)
        {
            if (this.UseWhiteList)
            {
                return this.WhiteList?.HasEntryWithID<EnemyDataBlock>(enemyID) ?? false;
            }
            else
            {
                return !(this.BlackList?.HasEntryWithID<EnemyDataBlock>(enemyID) ?? false);
            }
        }

        /// <summary>
        /// Returns whether or not the given enemy datablock name is valid.
        /// </summary>
        /// <param name="enemyDBName">The enemy datablock name.</param>
        /// <returns><see langword="true"/> if the enemy db name is valid, otherwise
        /// <see langword="false"/></returns>
        public bool IsValid(string enemyDBName)
        {
            if (this.UseWhiteList)
            {
                return this.WhiteList?.HasEntryWithName<EnemyDataBlock>(enemyDBName) ?? false;
            }
            else
            {
                return !(this.BlackList?.HasEntryWithName<EnemyDataBlock>(enemyDBName) ?? false);
            }
        }
    }
}
