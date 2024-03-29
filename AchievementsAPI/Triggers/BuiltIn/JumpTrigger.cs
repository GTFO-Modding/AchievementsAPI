﻿using AchievementsAPI.Managers;
using AchievementsAPI.Progress;
using AchievementsAPI.Triggers.Attributes;
using HarmonyLib;

namespace AchievementsAPI.Triggers.BuiltIn
{
    [TriggerPatch(typeof(Patches))]
    public class JumpTrigger : AchievementTrigger
    {
        public const string ID = "Jump";

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
            [HarmonyPatch(typeof(PLOC_Jump), nameof(PLOC_Jump.Enter))]
            [HarmonyPostfix]
            public static void Jump(PLOC_Jump __instance)
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
