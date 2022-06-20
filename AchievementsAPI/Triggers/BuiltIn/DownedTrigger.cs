using AchievementsAPI.Managers;
using AchievementsAPI.Progress;
using HarmonyLib;

namespace AchievementsAPI.Triggers.BuiltIn
{
    [TriggerPatches(typeof(Patches))]
    public class DownedTrigger : AchievementTrigger
    {
        public const string ID = "PlayerDown";

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
            [HarmonyPatch(typeof(PLOC_Downed), nameof(PLOC_Downed.Enter))]
            [HarmonyPostfix]
            public static void Down(PLOC_Downed __instance)
            {
                if (!__instance.m_owner.IsLocallyOwned)
                {
                    return;
                }

                AchievementManager.ActivateTrigger(ID);
            }
        }
    }
}
