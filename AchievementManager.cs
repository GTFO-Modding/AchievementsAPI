using AchievementsAPI.Utilities;
using System;
using System.Collections.Generic;
using MTFO.Managers;
using System.IO;
using System.Text.Json;
using AchievementsAPI.Registries;
using Player;
using AchievementsAPI.Progress;

namespace AchievementsAPI
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
        /// This manager's registry handler.
        /// <para>
        /// Use this to register Achievements, Conditions, or Triggers.
        /// </para>
        /// </summary>
        public static RegistryHandler Registry { get; } = new();

        /// <summary>
        /// The OnAchievementUnlocked event is invoked when an achievement is unlocked.
        /// </summary>
        public static event Action<AchievementDefinition> OnAchievementUnlocked;

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
                foreach (AchievementDefinition achievement in Registry.Achievements)
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

            AchievementRegistry? registeredAchievements = Registry.Achievements;

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

        internal static void InvokeOnAchievementUnlocked(AchievementDefinition achievement)
        {
            try
            {
                OnAchievementUnlocked.Invoke(achievement);
            }
            catch (Exception ex)
            {
                L.Error("Error running OnAchievementUnlocked callback: " + ex);
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
