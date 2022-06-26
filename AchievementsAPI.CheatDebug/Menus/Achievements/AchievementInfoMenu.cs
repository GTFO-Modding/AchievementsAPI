using AchievementsAPI.Achievements;
using AchievementsAPI.Achievements.Extensions;
using AchievementsAPI.CheatDebug.Extensions;
using AchievementsAPI.Managers;
using Flaff.CheatMenuGUI.API.Menus;
using System;

namespace AchievementsAPI.CheatDebug.Menus.Achievements
{
    internal sealed class AchievementInfoMenu : CustomMenuDataBase
    {
        public static readonly string UID = "achievement.info".ToMenuID();

        private AchievementDefinition? achievement;

        public AchievementInfoMenu() : base("Achievement Info", UID)
        { }

        protected override MenuItemData[] GetItems()
        {
            return new MenuItemData[]
            {
                MenuItemData.CreateDummy("Name"),
                MenuItemData.CreateDummy("Description"),
                MenuItemData.CreateDummy("Points"),
                MenuItemData.CreateDummy("Progress"),
                MenuItemData.CreateMenuOpener("Completion", AchievementCompletionMenu.UID, this.achievement!.ID)
            };
        }

        protected override void OnUpdatePosition()
        {
            base.OnUpdatePosition();
            if (this.instance == null || this.achievement == null)
            {
                return;
            }

            switch (this.ItemPosition)
            {
                case 0:
                    this.instance.DisplayPopup("<b>Name</b>: " + this.achievement.Name, 5);
                    break;
                case 1:
                    this.instance.DisplayPopup("<b>Description</b>\n" + this.achievement.Description, 5);
                    break;
                case 2:
                    this.instance.DisplayPopup("<b>Progress</b>\n" + (this.achievement.GetProgress() * 100) + "%", 5);
                    break;
                case 3:
                    {
                        bool completed = this.achievement.IsCompleted();
                        this.instance.DisplayPopup($"<b>Completion Status</b>: <color=#{(completed ? "8F8" : "F88")}>{(completed ? "Completed" : "Not Completed")}</color>", 5);
                    }
                    break;
            }
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
