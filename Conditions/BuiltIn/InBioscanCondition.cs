using AchievementsAPI.Utilities;
using Player;
using SNetwork;

namespace AchievementsAPI.Conditions.BuiltIn
{
    public class InBioscanCondition : AchievementCondition<InBioscanCondition.CustomData>
    {
        public const string ID = "InBioscan";

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
                    return player.IsInBioscan;
                }) ?? false;
            }
        }
    }
}
