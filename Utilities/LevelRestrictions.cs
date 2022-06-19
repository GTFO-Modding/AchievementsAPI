using System;
using System.Collections.Generic;
using System.Linq;

namespace AchievementsAPI.Utilities
{
    public class LevelRestrictions
    {
        public bool UseWhiteList { get; set; }
        public List<LevelInfo> WhitelistedLevels { get; set; } = new();
        public List<LevelInfo> BlacklistedLevels { get; set; } = new();

        public bool IsValid(int expeditionIndex, eRundownTier tier)
        {
            if (this.UseWhiteList)
            {
                return this.WhitelistedLevels?.Any((level) =>
                    level.Tier == tier && level.ExpeditionIndex == expeditionIndex) ?? false;
            }
            else
            {
                return !(this.BlacklistedLevels?.Any((level) =>
                    level.Tier == tier && level.ExpeditionIndex == expeditionIndex) ?? false);
            }
        }
    }
}
