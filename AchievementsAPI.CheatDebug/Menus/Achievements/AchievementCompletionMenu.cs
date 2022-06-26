using AchievementsAPI.Achievements;
using AchievementsAPI.CheatDebug.Cheats.Achievements;
using AchievementsAPI.CheatDebug.Extensions;
using AchievementsAPI.Managers;
using Flaff.CheatMenuGUI.API.Menus;

namespace AchievementsAPI.CheatDebug.Menus.Achievements
{
    internal sealed class AchievementCompletionMenu : CustomMenuDataBase
    {
        public static readonly string UID = "achievement.completion".ToMenuID();

        private AchievementDefinition? achievement;

        public AchievementCompletionMenu() : base("Achievement Completion", UID)
        { }

        protected override MenuItemData[] GetItems()
        {
            return new MenuItemData[]
            {
                MenuItemData.CreateCheatActivator("Force Complete", ForceCompleteAchievementCheat.UID, this.achievement!.ID),
                MenuItemData.CreateCheatActivator("Reset Progress", ResetAchievementProgressCheat.UID, this.achievement!.ID),
            };
        }

        public override void SetCustomData(object?[] data)
        {
            this.achievement = RegistryManager.Achievements[(string?)data[0]!];
        }
        public override void ClearCustomData()
        {
            this.achievement = null;
        }
    }
}
