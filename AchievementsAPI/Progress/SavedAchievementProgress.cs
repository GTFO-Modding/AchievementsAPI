namespace AchievementsAPI.Progress
{
    internal sealed class SavedAchievementProgress
    {
        public string AchievementID { get; set; }
        public AchievementProgress Progress { get; set; }

        public SavedAchievementProgress()
        {
            this.AchievementID = string.Empty;
            this.Progress = new AchievementProgress();
        }
    }
}
