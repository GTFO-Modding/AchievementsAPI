using AchievementsAPI.Progress;

namespace AchievementsAPI.Triggers
{
    internal interface _IAchievementTriggerBase : IAchievementTriggerBase
    {
        void ResetProgress(IAchievementTriggerProgress progress);
        void Trigger(object?[] data, IAchievementTriggerProgress progress);
    }
}
