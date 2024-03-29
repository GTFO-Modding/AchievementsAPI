﻿using AchievementsAPI.Managers;
using AchievementsAPI.Progress;
using AchievementsAPI.Triggers.Attributes;
using AchievementsAPI.Utilities;
using Enemies;
using Gear;
using HarmonyLib;
using System.Collections.Generic;

namespace AchievementsAPI.Triggers.BuiltIn
{
    [TriggerPatch(typeof(Patches))]
    public sealed class EnemyBiotrackedTrigger : AchievementTrigger<EnemyBiotrackedTrigger.CustomData>
    {
        public const string ID = "EnemyBiotracked";

        protected override TriggerParameterList[]? Parameters
        {
            get
            {
                return new TriggerParameterList[]
                {
                    new TriggerParameterList(
                        new("isLocalPlayer", typeof(bool)),
                        new("isUnique", typeof(bool)),
                        new("enemy", typeof(EnemyAgent)))
                };
            }
        }

        public override string GetID() => ID;

        public override bool CanBeTriggered(object?[] data)
        {
            if (data.Length == 0 || data[0] is not bool wasMe)
            {
                return false;
            }
            if (data.Length == 1 || data[1] is not bool isUnique)
            {
                return false;
            }
            if (data.Length == 2 || data[2] is not EnemyAgent enemy)
            {
                return false;
            }
            return this.Data.IsValid(wasMe, isUnique, enemy);
        }

        public override void Trigger(object?[] data, AchievementTriggerProgress progress)
        {
            progress.TriggerCount++;
        }

        public sealed class CustomData : TriggerData
        {
            public PlayerRestrictions? Players { get; set; }
            public EnemyRestrictions? Enemies { get; set; }
            public bool UniqueOnly { get; set; }

            public bool IsValid(bool wasMe, bool unique, EnemyAgent enemy)
            {
                return 
                    (!this.UniqueOnly || unique) &&
                    (this.Players?.IsValid(wasMe) ?? true) &&
                    (this.Enemies?.IsValid(enemy) ?? true);
            }
        }

        [HarmonyPatch]
        private static class Patches
        {
            private static EnemyScanner? s_currentBioTracker;
            private static readonly List<EnemyAgent?> s_taggedEnemies = new();

            [HarmonyPatch(typeof(GS_InLevel), nameof(GS_InLevel.Exit))]
            [HarmonyPrefix]
            public static void ExitLevelPrefix()
            {
                s_taggedEnemies.Clear();
            }

            [HarmonyPatch(typeof(EnemyScanner), nameof(EnemyScanner.UpdateTagProgress))]
            [HarmonyPrefix]
            public static void ScanPrefix(EnemyScanner __instance)
            {
                s_currentBioTracker = __instance;
            }
            [HarmonyPatch(typeof(EnemyScanner), nameof(EnemyScanner.UpdateTagProgress))]
            [HarmonyPostfix]
            public static void ScanPostfix()
            {
                s_currentBioTracker = null;
            }

            [HarmonyPatch(typeof(ToolSyncManager), nameof(ToolSyncManager.WantToTagEnemy))]
            [HarmonyPrefix]
            public static void TryTagEnemy(EnemyAgent enemy)
            {
                s_taggedEnemies.RemoveAll((e) => e == null);
                bool unique = true;
                foreach (EnemyAgent? taggedEnemy in s_taggedEnemies)
                {
                    if (taggedEnemy == enemy)
                    {
                        unique = false;
                        break;
                    }
                }

                if (unique)
                {
                    s_taggedEnemies.Add(enemy);
                }

                if (s_currentBioTracker == null)
                {
                    AchievementManager.ActivateTrigger(ID, false, unique, enemy);
                    return;
                }

                for (int index = 0; index < s_currentBioTracker.m_taggableEnemies.Count; index++)
                {
                    if (enemy == s_currentBioTracker.m_taggableEnemies[index])
                    {
                        AchievementManager.ActivateTrigger(ID, true, unique, enemy);
                        break;
                    }
                }
            }
        }
    }
}
