using AchievementsAPI.Registries;
using System;

namespace AchievementsAPI.Conditions
{
    public interface IAchievementCondition : IRegisterable
    {
        ConditionData Data { get; set; }

        Type GetDataType();

        void Setup();
        bool IsMet();
    }
}
