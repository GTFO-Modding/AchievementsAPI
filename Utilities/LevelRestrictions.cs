using System;
using System.Collections.Generic;
using System.Linq;

namespace AchievementsAPI.Utilities
{
    public class LevelRestrictions
    {
        public bool UseWhiteList { get; set; }
        public List<LevelInfo> WhiteListedLevels { get; set; } = new();
        public List<LevelInfo> BlackListedLevels { get; set; } = new();

        public bool IsValid(int expeditionIndex, eRundownTier tier)
        {
            if (this.UseWhiteList)
            {
                return this.WhiteListedLevels?.Any((level) =>
                    level.Tier == tier && level.ExpeditionIndex == expeditionIndex) ?? false;
            }
            else
            {
                return !(this.BlackListedLevels?.Any((level) =>
                    level.Tier == tier && level.ExpeditionIndex == expeditionIndex) ?? false);
            }
        }
    }
}
