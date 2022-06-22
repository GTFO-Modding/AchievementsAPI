using AchievementsAPI.Utilities;
using SNetwork;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace AchievementsAPI.Conditions.BuiltIn
{
    public class LevelRestrictionsCondition : AchievementCondition<LevelRestrictionsCondition.CustomData>
    {
        public const string ID = "LevelRestrictions";

        public override bool IsMet()
        {
            pActiveExpedition? expedition = SNet.GetLocalCustomData<pActiveExpedition>();
            return this.Data.IsValid(expedition.expeditionIndex, expedition.tier);
        }

        public override string GetID()
        {
            return ID;
        }

        public sealed class CustomData : ConditionData
        {
            public LevelRestrictions? Restrictions { get; set; } = new();

            public bool IsValid(int expeditionIndex, eRundownTier tier)
            {
                return this.Restrictions?.IsValid(expeditionIndex, tier) ?? true;
            }
        }
    }
}
