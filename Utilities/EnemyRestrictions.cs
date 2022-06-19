using Enemies;
using System.Collections.Generic;

namespace AchievementsAPI.Utilities
{
    public sealed class EnemyRestrictions
    {
        public bool UseWhiteList { get; set; }
        public List<uint>? WhiteList { get; set; }
        public List<uint>? BlackList { get; set; }

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
