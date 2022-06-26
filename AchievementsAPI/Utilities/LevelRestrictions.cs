using System.Collections.Generic;
using System.Linq;

namespace AchievementsAPI.Utilities
{
    /// <summary>
    /// Restrictions for specific levels
    /// </summary>
    public class LevelRestrictions
    {
        /// <summary>
        /// Whether or not to use the whitelist instead of the blacklist.
        /// </summary>
        public bool UseWhiteList { get; set; }
        /// <summary>
        /// The levels that are valid.
        /// </summary>
        public List<LevelInfo> WhiteList { get; set; } = new();
        /// <summary>
        /// The levels that are invalid.
        /// </summary>
        public List<LevelInfo> BlackList { get; set; } = new();

        /// <summary>
        /// Returns whether or not the given expedition index and tier correspond
        /// with a level in the whitelist or not included in the blacklist.
        /// </summary>
        /// <param name="expeditionIndex">The Expedition Index</param>
        /// <param name="tier">The Expedition Tier.</param>
        /// <returns><see langword="true"/> if it's valid, otherwise
        /// <see langword="false"/></returns>
        public bool IsValid(int expeditionIndex, eRundownTier tier)
        {
            if (this.UseWhiteList)
            {
                return this.WhiteList?.Any((level) =>
                    level.Tier == tier && level.ExpeditionIndex == expeditionIndex) ?? false;
            }
            else
            {
                return !(this.BlackList?.Any((level) =>
                    level.Tier == tier && level.ExpeditionIndex == expeditionIndex) ?? false);
            }
        }
    }
}
