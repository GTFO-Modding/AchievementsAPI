using AchievementsAPI.Utilities;
using Player;
using SNetwork;

namespace AchievementsAPI.Conditions.BuiltIn
{
    public class IsCrouchedCondition : AchievementCondition<IsCrouchedCondition.CustomData>
    {
        public const string ID = "IsCrouched";

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
            public PlayerRestrictions? Restrictions { get; set; } = new();

            public bool IsValid()
            {
                return this.Restrictions?.CheckForOnePlayer((player) =>
                {
                    return player.Locomotion.m_currentStateEnum == PlayerLocomotion.PLOC_State.Crouch;
                }) ?? true;
            }
        }
    }
}
