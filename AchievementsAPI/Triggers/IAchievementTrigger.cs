using AchievementsAPI.Progress;

namespace AchievementsAPI.Triggers
{
    internal interface IAchievementTrigger : _IAchievementTriggerBase
    {
        void ResetProgress(AchievementTriggerProgress progress);
        void Trigger(object?[] data, AchievementTriggerProgress progress);
    }
    internal interface IAchievementTrigger<TProgressData> : _IAchievementTriggerBase
        where TProgressData : TriggerProgressData, new()
    {
        void ResetProgress(AchievementTriggerProgress<TProgressData> progress);
        void Trigger(object?[] data, AchievementTriggerProgress<TProgressData> progress);
    }
}
