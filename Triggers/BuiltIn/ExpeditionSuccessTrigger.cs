using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AchievementsAPI.Triggers.BuiltIn
{
    [TriggerPatches(typeof(Patches))]
    public class ExpeditionSuccessTrigger : AchievementTrigger
    {
        public const string ID = "CompleteExpedition";

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
            [HarmonyPatch(typeof(GS_ExpeditionSuccess), nameof(GS_ExpeditionSuccess.Enter))]
            [HarmonyPostfix]
            private static void OnExpeditionCompleted()
            {
                AchievementManager.ActivateTrigger(ID);
            }
        }
    }
}
