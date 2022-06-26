using AchievementsAPI.Achievements;
using AchievementsAPI.CheatDebug.Extensions;
using AchievementsAPI.Managers;
using Flaff.CheatMenuGUI.API.Menus;
using System;
using System.Collections.Generic;

namespace AchievementsAPI.CheatDebug.Menus.Achievements
{
    internal sealed class AchievementListMenu : CustomScrollableMenuDataBase
    {
        public static readonly string UID = "achievements.list".ToMenuID();

        public AchievementListMenu() : base("Achievements", UID)
        { }

        public override int VisibleItemCount => 13;
        public override int ItemMiddle => (this.VisibleItemCount / 2) + 1;

        protected override MenuItemData[] GetAllItems()
        {
            List<MenuItemData> items = new List<MenuItemData>();

            foreach (AchievementDefinition achievement in RegistryManager.Achievements)
            {
                items.Add(MenuItemData.CreateMenuOpener(achievement.Name, AchievementInfoMenu.UID, achievement));
            }
            return items.ToArray();
        }
    }
}
