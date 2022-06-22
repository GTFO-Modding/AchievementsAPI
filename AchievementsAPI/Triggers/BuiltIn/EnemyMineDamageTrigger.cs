using AchievementsAPI.Managers;
using AchievementsAPI.Progress;
using AchievementsAPI.Triggers.Attributes;
using AchievementsAPI.Utilities;
using Enemies;
using HarmonyLib;
#if IL2CPP_INTEROP
using Il2CppInterop.Runtime.Injection;
#else
using UnhollowerRuntimeLib;
#endif
using Player;
using UnityEngine;

namespace AchievementsAPI.Triggers.BuiltIn
{
    [TriggerPatch(typeof(Patches))]
    public sealed class EnemyMineDamageTrigger : AchievementTrigger<EnemyMineDamageTrigger.CustomData>
    {
        public const string ID = "EnemyMineDamage";

        public override string GetID()
        {
            return ID;
        }

        public override bool CanBeTriggered(object?[] data)
        {
            if (data.Length == 0 || data[0] is not EnemyAgent enemy)
            {
                return false;
            }
            if (data.Length == 1 || data[1] is not float damage)
            {
                return false;
            }

            return this.Data.IsValid(enemy, damage);
        }

        public override void Trigger(object?[] data, AchievementTriggerProgress progress)
        {
            progress.TriggerCount++;
        }

        public sealed class CustomData : TriggerData
        {
            public EnemyRestrictions? Enemies { get; set; }
            public DamageRestrictionsData? Damage { get; set; } = new();
            public ValueRestriction<bool> IsLethal { get; set; }

            public bool IsValid(EnemyAgent enemy, float damage)
            {
                if (!(this.Enemies?.IsValid(enemy) ?? true))
                {
                    return false;
                }

                if (!(this.Damage?.IsValid(damage) ?? true))
                {
                    return false;
                }

                if (this.IsLethal.Enabled && (enemy.Damage.Health - damage > 0))
                {
                    return false;
                }

                return true;
            }
        }

        public sealed class DamageRestrictionsData
        {
            public bool HasRestriction { get; set; }
            public MinMaxRestriction<float> Restriction { get; set; }

            public bool IsValid(float damage)
            {
                if (!this.HasRestriction)
                {
                    return true;
                }

                return this.Restriction.IsValid(damage);
            }
        }

        private sealed class MineOwnerTrackerComponent : MonoBehaviour
        {
            public MineOwnerTrackerComponent(nint ptr) : base(ptr)
            { }

            public PlayerAgent? m_player;

            static MineOwnerTrackerComponent()
            {
                ClassInjector.RegisterTypeInIl2Cpp<MineOwnerTrackerComponent>();
            }

            public static void Add(MineDeployerInstance_Detonate_Explosive mine, PlayerAgent owner)
            {
                MineOwnerTrackerComponent comp = mine.gameObject.AddComponent<MineOwnerTrackerComponent>();
                comp.m_player = owner;
            }
        }

        private static class Patches
        {
            [HarmonyPatch(typeof(MineDeployerInstance), nameof(MineDeployerInstance.OnSpawnData))]
            [HarmonyWrapSafe]
            [HarmonyPostfix]
            public static void MineDeployerInstanceSetup(MineDeployerInstance __instance)
            {
                MineDeployerInstance_Detonate_Explosive? explosive = __instance.m_detonation.TryCast<MineDeployerInstance_Detonate_Explosive>();

                if (explosive == null)
                {
                    return;
                }

                MineOwnerTrackerComponent.Add(explosive, __instance.Owner);
            }

            private static MineDeployerInstance_Detonate_Explosive? s_mine = null;

            [HarmonyPatch(typeof(MineDeployerInstance_Detonate_Explosive), nameof(MineDeployerInstance_Detonate_Explosive.DoExplode))]
            [HarmonyWrapSafe]
            [HarmonyPrefix]
            public static void ExplodePrefix(MineDeployerInstance_Detonate_Explosive __instance)
            {
                MineOwnerTrackerComponent? comp = __instance.GetComponent<MineOwnerTrackerComponent>();
                if (comp != null && comp.m_player != null && comp.m_player.IsLocallyOwned)
                {
                    s_mine = __instance;
                }
            }
            [HarmonyPatch(typeof(MineDeployerInstance_Detonate_Explosive), nameof(MineDeployerInstance_Detonate_Explosive.DoExplode))]
            [HarmonyWrapSafe]
            [HarmonyPostfix]
            public static void ExplodePostfix()
            {
                s_mine = null;
            }
            [HarmonyPatch(typeof(Dam_EnemyDamageBase), nameof(Dam_EnemyDamageBase.ExplosionDamage))]
            [HarmonyWrapSafe]
            [HarmonyPrefix]
            public static void EnemyExplosionDamage(Dam_EnemyDamageBase __instance, float dam)
            {
                if (s_mine == null)
                {
                    return;
                }

                AchievementManager.ActivateTrigger(ID, __instance.Owner, dam);
            }
        }
    }
}
