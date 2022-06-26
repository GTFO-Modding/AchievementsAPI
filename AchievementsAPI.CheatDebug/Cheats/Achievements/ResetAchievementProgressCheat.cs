﻿using AchievementsAPI.CheatDebug.Extensions;
using AchievementsAPI.Managers;
using AchievementsAPI.Utilities;
using Flaff.CheatMenuGUI.API.Cheats;

namespace AchievementsAPI.CheatDebug.Cheats.Achievements
{
    internal sealed class ResetAchievementProgressCheat : CustomCheatBase
    {
        public static readonly string UID = "achievements.reset".ToCheatID();

        public ResetAchievementProgressCheat() : base(UID)
        { }

        public override void ActivateFromKeybind(string?[] parameters)
        {
            if (parameters.Length == 0)
            {
                L.Warn($"Activate from Keybind for {nameof(ResetAchievementProgressCheat)} requires one parameter: an achievement id!");
                return;
            }
            else if (parameters[0] == null)
            {
                L.Warn($"Activate from Keybind for {nameof(ResetAchievementProgressCheat)} requires one parameter: an achievement id! Instead got null!");
                return;
            }
            else
            {
                this.Activate(new object[] { parameters[0]! });
            }
        }

        protected override void DoActivate(object?[] data)
        {
            if (data.Length == 0)
            {
                L.Warn($"Activate for {nameof(ResetAchievementProgressCheat)} requires one parameter: an achievement id string!");
                return;
            }
            else if (data[0] is not string)
            {
                L.Warn($"Activate for {nameof(ResetAchievementProgressCheat)} requires one parameter: an achievement id string! Instead got {(data[0] is null ? "null" : data[0]!.GetType().Name)}!");
                return;
            }

            AchievementManager.ResetAchievementProgress((string)data[0]!);
        }
    }
}
