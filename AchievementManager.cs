using AchievementsAPI.Utilities;
using System;
using System.Collections.Generic;
using MTFO.Managers;
using System.IO;
using System.Text.Json;
using AchievementsAPI.Registries;
using Player;

namespace AchievementsAPI
{
    public static class AchievementManager
    {
        public static RegistryHandler Registry { get; } = new();

        public static event Action<AchievementDefinition> OnAchievementUnlocked;

        private static string? s_achievementAPIFolder;
        private static string? s_achievementProgressPath;
        private static string? s_achievementDefinitionPath;
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

        public static string AchievementDefinitionPath
        {
            get
            {
                if (s_achievementDefinitionPath == null)
                {
                    s_achievementDefinitionPath = Path.Combine(FetchAchievementAPIFolder(), "achievements.json");
                }
                return s_achievementDefinitionPath;
            }
        }

        private static readonly List<AchievementInstance> s_achievements = new();

        public static void ActivateTrigger(string id, params object?[] data)
        {
            bool save = false;
            foreach (var achievement in s_achievements)
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

            var registeredAchievements = Registry.Achievements;

            foreach (var achievement in registeredAchievements)
            {
                bool found = false;
                foreach (var existingAchievement in s_achievements)
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
                PlayerManager.GetLocalPlayerAgent().Sound.Post(AK.EVENTS.APEX_PUZZLE_SOLVED);
                GuiManager.PlayerLayer.m_wardenIntel.SetIntelText("<size=200%>Achievement Unlocked!</size>\n<size=190%>" + achievement.Name + "</size>\n<size=150%>" + achievement.Description + "</size>");
                GuiManager.PlayerLayer.m_wardenIntel.SetVisible(true, 5f);
            };
        }
    }
}
