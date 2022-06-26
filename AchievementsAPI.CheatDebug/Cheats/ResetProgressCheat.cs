using AchievementsAPI.CheatDebug.Extensions;
using AchievementsAPI.Managers;
using Flaff.CheatMenuGUI.API.Cheats;
using System;

namespace AchievementsAPI.CheatDebug.Cheats
{
    internal sealed class ResetProgressCheat : CustomCheatBase
    {
        public static readonly string UID = "resetprogress".ToCheatID();

        public ResetProgressCheat() : base(UID)
        { }

        public override void ActivateFromKeybind(string?[] parameters)
        {
            this.Activate(Array.Empty<object>());
        }

        protected override void DoActivate(object?[] data)
        {
            AchievementManager.ResetProgress();
        }
    }
}
