using AchievementsAPI.Managers;
using AchievementsAPI.Progress;
using AchievementsAPI.Triggers.Attributes;
using HarmonyLib;

namespace AchievementsAPI.Triggers.BuiltIn
{
    [TriggerPatch(typeof(Patches))]
    public class ExpeditionFailTrigger : AchievementTrigger
    {
        public const string ID = "FailExpedition";

        public override string GetID()
        {
            return ID;
        }

        public override void Trigger(object?[] data, AchievementTriggerProgress progress)
        {
            progress.TriggerCount++;
        }
        private static class Patches
        {
            [HarmonyPatch(typeof(GS_ExpeditionFail), nameof(GS_ExpeditionFail.Enter))]
            [HarmonyPostfix]
            public static void ExpeditionFail()
            {
                AchievementManager.ActivateTrigger(ID);
            }
        }
    }
}
