using AchievementsAPI.Managers;
using AchievementsAPI.Progress;
using AchievementsAPI.Triggers.Attributes;
using AchievementsAPI.Utilities;
using Agents;
using HarmonyLib;
using Player;

namespace AchievementsAPI.Triggers.BuiltIn
{
    [TriggerPatch(typeof(Patches))]
    public sealed class ReviveTrigger : AchievementTrigger<ReviveTrigger.CustomData>
    {
        public const string ID = "Revive";

        public override string GetID()
        {
            return ID;
        }

        protected override TriggerParameterList[]? Parameters
        {
            get
            {
                return new TriggerParameterList[]
                {
                    new TriggerParameterList(
                        new TriggerParameterDefinition("revivedPlayer", typeof(PlayerAgent)))
                };
            }
        }

        public override bool CanBeTriggered(object?[] data)
        {
            if (data.Length == 0 || data[0] is not PlayerAgent player)
            {
                return false;
            }

            return this.Data.IsValid(player);
        }

        public override void Trigger(object?[] data, AchievementTriggerProgress progress)
        {
            progress.TriggerCount++;
        }

        public sealed class CustomData : TriggerData
        {
            public PlayerRestrictions? PlayerToRevive { get; set; }

            public bool IsValid(PlayerAgent player)
            {
                if (this.PlayerToRevive == null)
                {
                    return true;
                }

                return this.PlayerToRevive.IsValid(player);
            }
        }

        private static class Patches
        {
            [HarmonyPatch(typeof(AgentReplicatedActions), nameof(AgentReplicatedActions.PlayerReviveAction))]
            public static void DoRevive(PlayerAgent p_target)
            {
                AchievementManager.ActivateTrigger(ID, p_target);
            }
        }
    }
}
