using AchievementsAPI.CheatDebug.Cheats;
using AchievementsAPI.CheatDebug.Cheats.Achievements;
using AchievementsAPI.CheatDebug.Menus;
using AchievementsAPI.CheatDebug.Menus.Achievements;
using AchievementsAPI.Utilities;
using BepInEx;
using BepInEx.IL2CPP;
using Flaff.CheatMenuGUI.API;
using HarmonyLib;
using CheatMenuAPIConstants = Flaff.CheatMenuGUI.API.PluginConstants;

namespace AchievementsAPI.CheatDebug
{
    [BepInPlugin("AchievementsAPI.CheatDebug", "AchievementsAPI.CheatDebug", "1.0.0")]
    [BepInProcess("GTFO.exe")]
    [BepInDependency(AchievementsAPIConstants.GUID, BepInDependency.DependencyFlags.HardDependency)]
    [BepInDependency(CheatMenuAPIConstants.GUID, BepInDependency.DependencyFlags.HardDependency)]
    internal sealed class MainPlugin : BasePlugin
    {
        public override void Load()
        {
            new Harmony("AchievementsAPI.CheatDebug")
                .PatchAll();

            InitializeListener.Add(OnCheatMenuInitialized);
        }

        public static void OnCheatMenuInitialized()
        {
            CheatMenuInstance? instance = CheatMenuInstance.Instance;
            if (instance == null)
            {
                return;
            }

            instance.RegisterCustomAPIMenu<AchievementsAPIMenu>("AchievementsAPI");
            instance.RegisterCustomMenu<AchievementListMenu>();
            instance.RegisterCustomMenu<AchievementInfoMenu>();
            instance.RegisterCustomMenu<AchievementCompletionMenu>();
            instance.RegisterCustomCheat<ForceCompleteAchievementCheat>();
            instance.RegisterCustomCheat<ResetAchievementProgressCheat>();
            instance.RegisterCustomCheat<ResetProgressCheat>();
        }
    }
}