using System.Collections.Generic;

namespace AchievementsAPI.Expansions.GTFuckingXP.Utilities
{
    /// <summary>
    /// Restrictions for XPMod levels.
    /// </summary>
    public sealed class XPLevelRestrictions
    {
        /// <summary>
        /// Whether or not to use the whitelist or blacklist.
        /// </summary>
        public bool UseWhiteList { get; set; }
        /// <summary>
        /// Items allowed.
        /// </summary>
        public List<int>? WhiteList { get; set; }
        /// <summary>
        /// Items not allowed.
        /// </summary>
        public List<int>? BlackList { get; set; }

        /// <summary>
        /// Returns whether or not the xp level correspond
        /// with a xp level in the whitelist or not included in the blacklist.
        /// </summary>
        /// <param name="level">The XP Level</param>
        /// <returns><see langword="true"/> if it's valid, otherwise
        /// <see langword="false"/></returns>
        public bool IsValid(int level)
        {
            if (this.UseWhiteList)
            {
                return this.WhiteList?.Contains(level) ?? false;
            }
            else
            {
                return !(this.BlackList?.Contains(level) ?? false);
            }
        }
    }
}
