using HarmonyLib;
using SNetwork;
using System.Collections.Generic;
using System.Linq;

namespace AchievementsAPI.Triggers.BuiltIn
{
    [TriggerPatches(typeof(Patches))]
    public class ExpeditionFailTrigger : AchievementTrigger
    {
        public const string ID = "FailExpedition";

        public override string GetID()
        {
            return ID;
        }

        public override void Trigger(object?[] data, ref AchievementTriggerProgress progress)
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
