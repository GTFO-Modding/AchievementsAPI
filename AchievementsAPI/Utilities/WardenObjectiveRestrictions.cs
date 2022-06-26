using GameData;
using System.Collections.Generic;

namespace AchievementsAPI.Utilities
{
    /// <summary>
    /// Restrictions for specific warden objectives.
    /// </summary>
    public sealed class WardenObjectiveRestrictions
    {
        /// <summary>
        /// Whether or not to use the whitelist or blacklist.
        /// </summary>
        public bool UseWhiteList { get; set; }
        /// <summary>
        /// Warden Objectives allowed.
        /// </summary>
        public List<DatablockReference?>? WhiteList { get; set; }
        /// <summary>
        /// Warden Objectives not allowed.
        /// </summary>
        public List<DatablockReference?>? BlackList { get; set; }

        /// <summary>
        /// Returns whether or not the given warden objective datablock is valid by checking if it's in
        /// the whitelist or blacklist.
        /// </summary>
        /// <param name="wardenObjectiveDB">The warden objective DB to check.</param>
        /// <returns><see langword="true"/> if the warden objective DB is valid, otherwise
        /// <see langword="false"/></returns>
        public bool IsValid(WardenObjectiveDataBlock wardenObjectiveDB)
        {
            return this.IsValid(wardenObjectiveDB.persistentID) ||
                this.IsValid(wardenObjectiveDB.name);
        }

        /// <summary>
        /// Returns whether or not the given warden objective ID is valid by checking if it's in
        /// the whitelist or blacklist with it's persistentID.
        /// </summary>
        /// <param name="wardenObjectiveID">The warden objective ID to check.</param>
        /// <returns><see langword="true"/> if the warden objective ID is valid, otherwise
        /// <see langword="false"/></returns>
        public bool IsValid(uint wardenObjectiveID)
        {
            if (this.UseWhiteList)
            {
                return this.WhiteList?.HasEntryWithID<WardenObjectiveDataBlock>(wardenObjectiveID) ?? false;
            }
            else
            {
                return !(this.BlackList?.HasEntryWithID<WardenObjectiveDataBlock>(wardenObjectiveID) ?? false);
            }
        }

        /// <summary>
        /// Returns whether or not the given warden objective datablock name is valid.
        /// </summary>
        /// <param name="wardenObjectiveDBName">The warden objective datablock name.</param>
        /// <returns><see langword="true"/> if the warden objective db name is valid, otherwise
        /// <see langword="false"/></returns>
        public bool IsValid(string wardenObjectiveDBName)
        {
            if (this.UseWhiteList)
            {
                return this.WhiteList?.HasEntryWithName<WardenObjectiveDataBlock>(wardenObjectiveDBName) ?? false;
            }
            else
            {
                return !(this.BlackList?.HasEntryWithName<WardenObjectiveDataBlock>(wardenObjectiveDBName) ?? false);
            }
        }
    }
}
