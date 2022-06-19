using AchievementsAPI.Progress;
using HarmonyLib;

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

        public override void Trigger(object?[] data, AchievementTriggerProgress progress)
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
