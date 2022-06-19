using AchievementsAPI.Conditions.BuiltIn;
using AchievementsAPI.Triggers;
using AchievementsAPI.Triggers.BuiltIn;
using AchievementsAPI.Utilities;
using BepInEx;
using BepInEx.IL2CPP;
using BepInEx.Logging;
using GameData;
using HarmonyLib;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.Json;

namespace AchievementsAPI
{
    [BepInPlugin(PLUGIN_CONSTANTS.PLUGIN_GUID, PLUGIN_CONSTANTS.PLUGIN_NAME, PLUGIN_CONSTANTS.PLUGIN_VERSION)]
    [BepInDependency("com.dak.MTFO", BepInDependency.DependencyFlags.HardDependency)]
    internal partial class MainPlugin : BasePlugin
    {
        internal static ManualLogSource? LogSource;
        private static Harmony? s_harmony;

        public override void Load()
        {
            LogSource = this.Log;

            this.RegisterBuiltIns();

            s_harmony = new Harmony(PLUGIN_CONSTANTS.PLUGIN_GUID);
            s_harmony.PatchAll(typeof(BasicPatches));
        }

        public override bool Unload()
        {
            s_harmony!.UnpatchSelf();
            return base.Unload();
        }

        private class AchievementDefinitionFile
        {
            public List<AchievementDefinition> Achievements { get; set; } = new();
        }

        private static class BasicPatches
        {
            [HarmonyPatch(typeof(GS_Offline), nameof(GS_Offline.Enter))]
            [HarmonyPostfix]
            [HarmonyWrapSafe]
            public static void PatchTriggers()
            {
                L.Info("Patching triggers!");
                foreach (var triggerInfo in AchievementManager.Registry.Triggers)
                {
                    L.Debug($"Patching '{triggerInfo.ID}'");
                    var patchesAttribute = triggerInfo.Type.GetCustomAttribute<TriggerPatchesAttribute>();
                    if (patchesAttribute != null)
                    {
                        s_harmony!.PatchAll(patchesAttribute.GetPatchType());
                    }
                }
            }

            [HarmonyPatch(typeof(GameDataInit), nameof(GameDataInit.Initialize))]
            [HarmonyPostfix]
            [HarmonyWrapSafe]
            public static void InitializePatch()
            {

                string path = AchievementManager.AchievementDefinitionsPath;
                AchievementManager.Registry.Achievements.UnRegisterAll();

                if (!File.Exists(path))
                {
                    // todo: add path
                    L.Warn("TODO: Add ability to create template achievements");

                    return;
                }

                JsonSerializerOptions options = new()
                {
                    AllowTrailingCommas = true,
                    WriteIndented = true,
                    ReadCommentHandling = JsonCommentHandling.Skip
                };

                string contents = File.ReadAllText(path);
                AchievementDefinitionFile file = JsonSerializer.Deserialize<AchievementDefinitionFile>(contents, options) ?? new();

                if (file.Achievements == null)
                {
                    file.Achievements = new();
                    File.WriteAllText(path, JsonSerializer.Serialize(file, options));
                    return;
                }

                L.Info($"Loaded '{file.Achievements.Count}' achievement{(file.Achievements.Count != 1 ? "s" : "")}");

                foreach (var achievement in file.Achievements)
                {
                    L.Debug($"Registering Achievement '{achievement.ID}' ({achievement.Name})");
                    AchievementManager.Registry.Achievements.Register(achievement);
                }

                L.Info("Loading Progress!");
                AchievementManager.LoadProgress();
            }
        }
    }
}
