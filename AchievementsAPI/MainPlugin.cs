using AchievementsAPI.Achievements;
using AchievementsAPI.Managers;
using AchievementsAPI.Triggers;
using AchievementsAPI.Triggers.Attributes;
using AchievementsAPI.Triggers.Registries;
using AchievementsAPI.Utilities;
using BepInEx;
using BepInEx.Unity.IL2CPP;
using BepInEx.Logging;
using GameData;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.Json;

namespace AchievementsAPI
{
    [BepInPlugin(PLUGIN_CONSTANTS.PLUGIN_GUID, PLUGIN_CONSTANTS.PLUGIN_NAME, PLUGIN_CONSTANTS.PLUGIN_VERSION)]
    [BepInProcess("GTFO.exe")]
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
            public static void InitializeTriggers()
            {
                L.Info("Initializing triggers!");
                foreach (TriggerElementFactorySettings? triggerInfo in RegistryManager.Triggers)
                {
                    L.Debug($"Initializing '{triggerInfo.ID}'");
                    Type triggerType = triggerInfo.Type;

                    // setup trigger patches
                    IEnumerable<TriggerPatchAttribute> patchAttributes = triggerType.GetCustomAttributes<TriggerPatchAttribute>();
                    foreach (TriggerPatchAttribute patchAttribute in patchAttributes)
                    {
                        Type patchType = patchAttribute.GetPatchType();
                        L.Debug($"> Patching '{patchType.FullName}'");
                        s_harmony!.PatchAll(patchType);
                    }

                    // setup trigger using methods
                    IEnumerable<TriggerSetupMethodAttribute> setupAttributes = triggerType.GetCustomAttributes<TriggerSetupMethodAttribute>();

                    foreach (TriggerSetupMethodAttribute setupAttribute in setupAttributes)
                    {
                        string setupMethodName = setupAttribute.GetMethodName();
                        MethodInfo? setupMethod = triggerType.GetMethod(setupMethodName, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static, Array.Empty<Type>());

                        if (setupMethod == null)
                        {
                            L.Warn($"Trigger '{triggerInfo.ID}' specifies a setup method named {setupMethodName}, but no method was found. Make sure your method is static, and takes no arguments.");
                            continue;
                        }

                        try
                        {
                            L.Debug("> Preparing to call method '" + setupMethodName + "'");
                            setupMethod.Invoke(null, null);
                            L.Debug("> Successfully ran setup method '" + setupMethodName + "'");
                        }
                        catch (TargetInvocationException invokeException)
                        {
                            L.Error($"Error whilst calling setup method named '{setupMethodName}' on trigger with id '{triggerInfo.ID}': {invokeException.InnerException?.ToString() ?? "Unknown Exception"}");
                        }
                        catch (InvalidOperationException invalidMethodException)
                        {
                            L.Error($"Trigger '{triggerInfo.ID}' specifies a setup method named {setupMethodName}, but is not callable. Exception: {invalidMethodException}");
                        }
                        catch (Exception otherException)
                        {
                            L.Error($"Error attempting to call setup method named '{setupMethodName}' on trigger with id '{triggerInfo.ID}': {otherException}");
                        }
                    }
                }
            }

            [HarmonyPatch(typeof(GameDataInit), nameof(GameDataInit.Initialize))]
            [HarmonyPrefix]
            [HarmonyWrapSafe]
            public static void UnInitializePatch()
            {
                RegistryManager.Achievements.UnRegisterAll();

                // more unregistry logic can be added here later...
            }

            [HarmonyPatch(typeof(GameDataInit), nameof(GameDataInit.Initialize))]
            [HarmonyPostfix]
            [HarmonyWrapSafe]
            public static void InitializePatch()
            {
                InitializeAchievements();

                L.Info("Requesting Achievement Register from AchievementManager.");
                AchievementManager.InvokeRegisterAchievements();

                InitializeAchievementProgress();
            }

            private static void InitializeAchievements()
            {
                string path = AchievementManager.AchievementDefinitionsPath;

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

                L.Info($"Loaded '{file.Achievements.Count}' achievement{(file.Achievements.Count != 1 ? "s" : "")} from file.");

                foreach (AchievementDefinition? achievement in file.Achievements)
                {
                    L.Debug($"Registering Achievement '{achievement.ID}' ({achievement.Name})");
                    RegistryManager.Achievements.Register(achievement);
                }
            }

            private static void InitializeAchievementProgress()
            {
                L.Info("Loading Progress!");
                AchievementManager.LoadProgress();
            }
        }
    }
}
