using AchievementsAPI.Utilities;
using System;
using System.Collections.Generic;
using MTFO.Managers;
using System.IO;
using System.Text.Json;
using Player;
using AchievementsAPI.Progress;
using AchievementsAPI.Achievements;
using AchievementsAPI.Achievements.Registries;

namespace AchievementsAPI.Managers
{
    /// <summary>
    /// The AchievementManager is responsible for tracking the achievements, loading achievements
    /// and their progress, as well as managing specific parts about achievements like Triggers.
    /// <para>
    /// It also contains a registry handler used for registering Achievements, Conditions, and
    /// Triggers.
    /// </para>
    /// </summary>
    public static class AchievementManager
    {
        /// <summary>
        /// The OnAchievementUnlocked event is invoked when an achievement is unlocked.
        /// </summary>
        public static event Action<AchievementDefinition>? OnAchievementUnlocked;

        /// <summary>
        /// A listener that gets invoked whenever <see cref="AchievementDefinition"/>s should
        /// be registered. This will ensure you achievements are added before achievement progress
        /// is loaded, which could break if your achievement was not registered in time.
        /// </summary>
        public static event Action? RegisterAchievementsListener;

        private static string? s_achievementAPIFolder;
        private static string? s_achievementProgressPath;
        private static string? s_achievementDefinitionsPath;
        private static string FetchAchievementAPIFolder()
        {
            if (s_achievementAPIFolder == null)
            {
                s_achievementAPIFolder = Path.Combine(ConfigManager.CustomPath, "AchievementAPI");
                if (!Directory.Exists(s_achievementAPIFolder))
                {
                    Directory.CreateDirectory(s_achievementAPIFolder);
                }
            }
            return s_achievementAPIFolder;
        }

        /// <summary>
        /// The path to the Achievement Progress json file.
        /// </summary>
        public static string AchievementProgressPath
        {
            get
            {
                if (s_achievementProgressPath == null)
                {
                    s_achievementProgressPath = Path.Combine(FetchAchievementAPIFolder(), "achievement-progress.json");
                }
                return s_achievementProgressPath;
            }
        }

        /// <summary>
        /// The path to the Achievement Definitions json file.
        /// </summary>
        public static string AchievementDefinitionsPath
        {
            get
            {
                if (s_achievementDefinitionsPath == null)
                {
                    s_achievementDefinitionsPath = Path.Combine(FetchAchievementAPIFolder(), "achievements.json");
                }
                return s_achievementDefinitionsPath;
            }
        }

        /// <summary>
        /// The current achievement points
        /// </summary>
        public static int AchievementPoints
        {
            get
            {
                int points = 0;
                foreach (AchievementInstance achievement in s_achievements)
                {
                    if (achievement.Completed)
                    {
                        points += achievement.Definition.AchievementPoints;
                    }
                }
                return points;
            }
        }

        private static readonly List<AchievementInstance> s_achievements = new();

        /// <summary>
        /// Activates/Triggers a specific trigger.
        /// </summary>
        /// <param name="id">The ID of the trigger.</param>
        /// <param name="data">The Data associated with the trigger.</param>
        public static void ActivateTrigger(string id, params object?[] data)
        {
            bool save = false;
            foreach (AchievementInstance? achievement in s_achievements)
            {
                achievement.ActivateTrigger(id, data, ref save);
            }

            if (save)
            {
                SaveProgress();
            }
        }

        internal static void LoadProgress()
        {
            s_achievements.Clear();
            string path = AchievementProgressPath;
            if (!File.Exists(path))
            {
                foreach (AchievementDefinition achievement in RegistryManager.Achievements)
                {
                    s_achievements.Add(new AchievementInstance(achievement, new()));
                }

                SaveProgress();
                return;
            }

            AchievementProgressFile file = JsonSerializer.Deserialize<AchievementProgressFile>(File.ReadAllText(path), new JsonSerializerOptions()
            {
                AllowTrailingCommas = true,
                ReadCommentHandling = JsonCommentHandling.Skip
            }) ?? new AchievementProgressFile();

            file.LoadAchievements(s_achievements);

            AchievementRegistry? registeredAchievements = RegistryManager.Achievements;

            foreach (AchievementDefinition? achievement in registeredAchievements)
            {
                bool found = false;
                foreach (AchievementInstance? existingAchievement in s_achievements)
                {
                    if (existingAchievement.Definition.ID == achievement.ID)
                    {
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    s_achievements.Add(new AchievementInstance(achievement, new()));
                }
            }

            
        }

        /// <summary>
        /// Save the current achievement progress.
        /// </summary>
        public static void SaveProgress()
        {
            string path = AchievementProgressPath;
            AchievementProgressFile file = new(s_achievements);

            File.WriteAllText(path, JsonSerializer.Serialize(file, new JsonSerializerOptions()
            {
                WriteIndented = true,
            }));
        }

        /// <summary>
        /// Resets the progress of all achievements!
        /// </summary>
        public static void ResetProgress()
        {
            foreach (AchievementInstance instance in s_achievements)
            {
                instance.ResetProgress(dontSave: true);
            }

            SaveProgress();
        }

        /// <summary>
        /// Resets all progress for the achievement with the given ID.
        /// </summary>
        /// <param name="achievementID">The ID of the achievement to reset progress of.</param>
        public static void ResetAchievementProgress(string achievementID)
        {
            foreach (AchievementInstance instance in s_achievements)
            {
                if (instance.Definition.ID == achievementID)
                {
                    instance.ResetProgress();
                    return;
                }
            }

            L.Warn($"Attempted to reset progress for achievement with id '{achievementID}', but no such achievement exists!");
        }

        internal static void ForceCompleteAchievement(string achievementID)
        {
            foreach (AchievementInstance instance in s_achievements)
            {
                if (instance.Definition.ID == achievementID)
                {
                    bool isCompleted = instance.Completed;
                    instance.ForceComplete();
                    if (!isCompleted)
                    {
                        InvokeOnAchievementUnlocked(instance.Definition);
                    }
                    return;
                }
            }

            L.Warn($"Attempted to force complete achievement with id '{achievementID}', but no such achievement exists!");
        }

        /// <summary>
        /// Resets all progress for all triggers with the given ID in the
        /// achievement with the given ID.
        /// </summary>
        /// <param name="achievementID">The ID of the achievement to reset progress of.</param>
        /// <param name="triggerID">The ID of the triggers to reset the progress of.</param>
        public static void ResetTriggerProgress(string achievementID, string triggerID)
        {
            foreach (AchievementInstance instance in s_achievements)
            {
                if (instance.Definition.ID == achievementID)
                {
                    instance.ResetProgress(triggerID);
                    return;
                }
            }
            L.Warn($"Attempted to reset progress for triggers with id '{triggerID}' in achievement with id '{achievementID}', but no such achievement exists!");
        }

        /// <summary>
        /// Resets all progress for all trigger with the given ID and increment
        /// in the achievement with the given ID.
        /// </summary>
        /// <param name="achievementID">The ID of the achievement to reset progress of.</param>
        /// <param name="triggerID">The ID of the trigger to reset the progress of.</param>
        /// <param name="increment">The increment of the trigger to reset the progress of.</param>
        public static void ResetTriggerProgress(string achievementID, string triggerID, uint increment)
        {
            foreach (AchievementInstance instance in s_achievements)
            {
                if (instance.Definition.ID == achievementID)
                {
                    instance.ResetProgress(triggerID, increment);
                    return;
                }
            }
            L.Warn($"Attempted to reset progress for trigger with id '{triggerID}' and increment {increment} in achievement with id '{achievementID}', but no such achievement exists!");
        }

        /// <summary>
        /// Returns whether the achievement with the given ID is completed.
        /// </summary>
        /// <param name="id">The ID of the achievement.</param>
        /// <returns><see langword="false"/> if the achievement wasn't completed, or no such
        /// achievement with id <paramref name="id"/> exists, otherwise <see langword="true"/>.
        /// </returns>
        public static bool IsAchievementCompleted(string id)
        {
            foreach (AchievementInstance instance in s_achievements)
            {
                if (instance.Definition.ID == id)
                {
                    return instance.Completed;
                }
            }
            L.Warn($"Attempted to get completion status for achievement with id '{id}', but no such achievement exists!");
            return false;
        }

        /// <summary>
        /// Returns the progress of the given achievement.
        /// </summary>
        /// <param name="id">The ID of the achievement.</param>
        /// <returns>A value between 0 and 1 [inclusive], where 0 means 0% completed, and
        /// 1 means 100% completed. If no such achievement with id <paramref name="id"/>
        /// exists, this method returns <see cref="double.NaN"/>.
        /// </returns>
        public static double GetAchievementProgress(string id)
        {
            foreach (AchievementInstance instance in s_achievements)
            {
                if (instance.Definition.ID == id)
                {
                    return instance.GetProgress();
                }
            }
            L.Warn($"Attempted to get the progress for achievement with id '{id}', but no such achievement exists!");
            return double.NaN;
        }

        internal static void InvokeOnAchievementUnlocked(AchievementDefinition achievement)
        {
            try
            {
                OnAchievementUnlocked?.Invoke(achievement);
            }
            catch (Exception ex)
            {
                L.Error("Error running OnAchievementUnlocked callback: " + ex);
            }
        }

        internal static void InvokeRegisterAchievements()
        {
            try
            {
                RegisterAchievementsListener?.Invoke();
            }
            catch (Exception ex)
            {
                L.Error("Error running RegisterAchievementsListener callback: " + ex);
            }
        }

        static AchievementManager()
        {
            OnAchievementUnlocked += (achievement) =>
            {
                L.Debug($"Unlocked achievement '{achievement.Name}' - {achievement.Description}");
                if (achievement.CompletionSound.Enabled)
                {
                    uint soundID = AK.EVENTS.APEX_PUZZLE_SOLVED;
                    if (achievement.CompletionSound.SoundID != 0U)
                    {
                        soundID = achievement.CompletionSound.SoundID;
                    }

                    PlayerManager.GetLocalPlayerAgent().Sound.Post(soundID);
                }
                GuiManager.PlayerLayer.m_wardenIntel.SetIntelText("<size=200%>Achievement Unlocked!</size>\n<size=190%>" + achievement.Name + "</size>\n<size=150%>" + achievement.Description + "</size>");
                GuiManager.PlayerLayer.m_wardenIntel.SetVisible(true, 5f);
            };
        }
    }
}
