using AchievementsAPI.Conditions;
using AchievementsAPI.Registries;
using System;

namespace AchievementsAPI.Triggers
{
    public interface IAchievementTrigger : IRegisterable
    {
        int Count { get; set; }
        TriggerData Data { get; set; }
        ConditionOverrides ConditionOverrides { get; set; }

        Type GetDataType();

        void Trigger(object?[] data, ref AchievementTriggerProgress progress);
        void Setup();
    }
}
