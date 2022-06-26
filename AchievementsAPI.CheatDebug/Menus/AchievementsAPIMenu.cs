using AchievementsAPI.CheatDebug.Cheats;
using AchievementsAPI.CheatDebug.Extensions;
using AchievementsAPI.CheatDebug.Menus.Achievements;
using Flaff.CheatMenuGUI.API.Menus;

namespace AchievementsAPI.CheatDebug.Menus
{
    internal sealed class AchievementsAPIMenu : CustomMenuDataBase
    {
        public static readonly string UID = "main".ToMenuID();

        public AchievementsAPIMenu() : base("AchievementsAPI", UID)
        { }

        protected override MenuItemData[] GetItems()
        {
            return new MenuItemData[]
            {
                MenuItemData.CreateMenuOpener("Achievements", AchievementListMenu.UID),
                MenuItemData.CreateCheatActivator("Reset Progress", ResetProgressCheat.UID)
            };
        }
    }
}
