using AchievementsAPI.Achievements;
using AchievementsAPI.Managers;
using AchievementsAPI.Utilities;
using System.Collections.Generic;

namespace AchievementsAPI.Progress
{
    internal sealed class AchievementProgressFile
    {
        public List<SavedAchievementProgress> Achievements { get; set; }

        public AchievementProgressFile()
        {
            this.Achievements = new();
        }

        internal AchievementProgressFile(List<AchievementInstance> achievements) : this()
        {
            foreach (AchievementInstance? instance in achievements)
            {
                this.Achievements.Add(new SavedAchievementProgress()
                {
                    AchievementID = instance.Definition.ID,
                    Progress = instance.Progress
                });
            }
        }

        internal void LoadAchievements(List<AchievementInstance> achievements)
        {
            foreach (SavedAchievementProgress? achievement in this.Achievements)
            {
                if (!RegistryManager.Achievements.TryGetEntry(achievement.AchievementID, out AchievementDefinition? definition))
                {
                    L.Warn($"Achievement Progress file references achievement with id '{achievement.AchievementID}', yet no such achievement has been registered! It's entry will be removed if the file is saved by progressing other achievements!");
                    continue;
                }
                achievements.Add(new AchievementInstance(definition, achievement.Progress));
            }
        }
    }
}
