using AchievementsAPI.Utilities;
using Player;
using SNetwork;

namespace AchievementsAPI.Conditions.BuiltIn
{
    public sealed class IsDownedCondition : AchievementCondition<IsDownedCondition.CustomData>
    {
        public const string ID = "IsDowned";

        public override string GetID()
        {
            return ID;
        }

        public override bool IsMet()
        {
            return this.Data.IsValid();
        }

        public sealed class CustomData : ConditionData
        {
            public PlayerRestrictions? Restrictions { get; set; }

            public bool IsValid()
            {
                return this.Restrictions?.CheckForOnePlayer((player) => player.Locomotion.m_currentStateEnum == PlayerLocomotion.PLOC_State.Downed) ?? true;
            }
        }
    }
}
