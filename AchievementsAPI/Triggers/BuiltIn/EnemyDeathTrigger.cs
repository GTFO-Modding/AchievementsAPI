using AchievementsAPI.Managers;
using AchievementsAPI.Progress;
using AchievementsAPI.Triggers.Attributes;
using AchievementsAPI.Utilities;
using Agents;
using Enemies;
using HarmonyLib;
using Player;
using System.Diagnostics.CodeAnalysis;

namespace AchievementsAPI.Triggers.BuiltIn
{
    [TriggerPatch(typeof(Patches))]
    public sealed class EnemyDeathTrigger : AchievementTrigger<EnemyDeathTrigger.CustomData>
    {
        public const string ID = "EnemyDeath";

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
                        new("enemyKilled", typeof(EnemyAgent)),
                        new("playerWhoKilled", typeof(PlayerAgent)))
                };
            }
        }

        private static bool TryGetTriggerData(object?[] data, [NotNullWhen(true)] out EnemyAgent? enemy, [NotNullWhen(true)] out PlayerAgent? player)
        {
            player = null;
            enemy = null;

            if (data.Length == 0 || data[0] is not EnemyAgent e)
            {
                return false;
            }
            if (data.Length == 1 || data[1] is not PlayerAgent p)
            {
                return false;
            }

            enemy = e;
            player = p;
            return true;
        }

        public override bool CanBeTriggered(object?[] data)
        {
            if (!TryGetTriggerData(data, out EnemyAgent? enemy, out PlayerAgent? player))
            {
                return false;
            }

            return this.Data.IsValid(enemy, player);
        }

        public override void Trigger(object?[] data, AchievementTriggerProgress progress)
        {
            progress.TriggerCount++;
        }

        public sealed class CustomData : TriggerData
        {
            public PlayerRestrictions? Players { get; set; } = new();
            public EnemyRestrictions? Enemies { get; set; } = new();

            public bool IsValid(EnemyAgent enemy, PlayerAgent player)
            {
                if (!(this.Enemies?.IsValid(enemy) ?? true))
                {
                    return false;
                }
                if (!(this.Players?.IsValid(player) ?? true))
                {
                    return false;
                }
                return true;
            }
        }


        private static class Patches
        {
            [HarmonyPatch(typeof(Dam_EnemyDamageBase), nameof(Dam_EnemyDamageBase.ProcessReceivedDamage))]
            [HarmonyPrefix]
            public static void ProcessReceivedDamage(Dam_EnemyDamageBase __instance, float damage, Agent damageSource)
            {
                bool lethal = __instance.WillDamageKill(damage);
                if (!lethal)
                {
                    return;
                }
                L.Debug("[EnemyDeath] Detected lethal kill");

                if (damageSource == null)
                {
                    L.Warn("[EnemyDeath] Detected lethal damage, but damage source agent was null!");
                    return;
                }

                PlayerAgent? playerSource = damageSource.TryCast<PlayerAgent>();
                if (playerSource == null)
                {
                    L.Warn("[EnemyDeath] Detected lethal damage, but damage source agent was not a player.");
                    return;
                }

                L.Debug("[EnemyDeath] Activating Trigger!");

                AchievementManager.ActivateTrigger(ID, __instance.Owner, playerSource);
            }
        }
    }
}
