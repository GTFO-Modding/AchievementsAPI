using AchievementsAPI.Managers;
using AchievementsAPI.Progress;
using AchievementsAPI.Triggers;
using AchievementsAPI.Triggers.Attributes;
using AchievementsAPI.Utilities;
using Agents;
using EEC.CustomAbilities.Explosion;
using EEC.EnemyCustomizations.Shared;
using Enemies;
using GTFO.API;
using HarmonyLib;
using Player;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace AchievementsAPI.Expansions.EEC.Triggers
{
    [TriggerPatch(typeof(Patches))]
    [TriggerSetupMethod(nameof(SetupNetwork))]
    public class EnemyExplodeTrigger : AchievementTrigger<EnemyExplodeTrigger.CustomData>
    {
        public const string ID = "EEC.EnemyExplode";
        private const string NETWORK_EVENT_ID = "AchievementAPI.Triggers." + ID + ".Packet";

        public override string GetID() => ID;

        public override void Trigger(object?[] data, AchievementTriggerProgress progress)
        {
            if (data.Length == 0 || data[0] is not PacketData packetData)
            {
                return;
            }

            if (!this.Data.IsValid(packetData))
            {
                return;
            }

            progress.TriggerCount++;
        }

        private static void OnReceiveTrigger(ulong _, PacketData packet)
        {
            AchievementManager.ActivateTrigger(ID, packet);
        }

        static void SetupNetwork()
        {
            NetworkAPI.RegisterEvent<PacketData>(NETWORK_EVENT_ID, OnReceiveTrigger);
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct PacketData
        {
            public uint EnemyID;
            public PacketGroupList DamageTargets;

            public PacketData(uint enemyID, IEnumerable<(IDamageable damageable, float damage)> targets)
            {
                this.EnemyID = enemyID;

                List<(PlayerAgent player, float damage)> players = new();
                List<(EnemyAgent enemy, float damage)> enemies = new();

                foreach ((IDamageable damageable, float damage) target in targets)
                {
                    Agent? baseAgent = target.damageable.GetBaseAgent();
                    if (baseAgent == null)
                    {
                        continue;
                    }

                    PlayerAgent? player = baseAgent?.TryCast<PlayerAgent>();
                    if (player != null)
                    {
                        bool exists = false;
                        for (int index = 0; index < players.Count; index++)
                        {
                            if (players[index].player == player)
                            {
                                (PlayerAgent player, float damage) update = players[index];
                                update.damage += target.damage;
                                players[index] = update;
                                exists = true;
                                break;
                            }
                        }

                        if (!exists)
                        {
                            players.Add((player, target.damage));
                        }
                        continue;
                    }

                    EnemyAgent? enemy = baseAgent?.TryCast<EnemyAgent>();
                    if (enemy != null)
                    {
                        bool exists = false;
                        for (int index = 0; index < enemies.Count; index++)
                        {
                            if (enemies[index].enemy == enemy)
                            {
                                (EnemyAgent enemy, float damage) update = enemies[index];
                                update.damage += target.damage;
                                enemies[index] = update;
                                exists = true;
                                break;
                            }
                        }

                        if (!exists)
                        {
                            enemies.Add((enemy, target.damage));
                        }
                        continue;
                    }
                }

                this.DamageTargets = new(enemies.ToArray(), players.ToArray());
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct PacketGroupList
        {
            private const int PLAYER_SIZE_CONST = 4;
            private const int ENEMY_SIZE_CONST = 64;

            public int PlayerCount;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = PLAYER_SIZE_CONST)]
            private PacketGroupData<ulong>[] m_Players;
            public int EnemyCount;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = ENEMY_SIZE_CONST)]
            private PacketGroupData<uint>[] m_Enemies;

            public PacketGroupList((EnemyAgent enemy, float damage)[] enemies, 
                (PlayerAgent player, float damage)[] players)
            {
                this.PlayerCount = Math.Min(players.Length, PLAYER_SIZE_CONST);
                this.EnemyCount = Math.Max(enemies.Length, ENEMY_SIZE_CONST);
                this.m_Players = new PacketGroupData<ulong>[PLAYER_SIZE_CONST];
                this.m_Enemies = new PacketGroupData<uint>[ENEMY_SIZE_CONST];

                for (int index = 0; index < this.EnemyCount; index++)
                {
                    this.m_Enemies[index] = new PacketGroupData<uint>(enemies[index].enemy.EnemyDataID, enemies[index].damage);
                }
                for (int index = 0; index < this.PlayerCount; index++)
                {
                    this.m_Players[index] = new PacketGroupData<ulong>(players[index].player.Owner.Lookup, players[index].damage);
                }
            }

            private static PacketGroupData<T> GetEntry<T>(PacketGroupData<T>[] group, int index, int count)
                where T : struct
            {
                if (index < 0 || index >= count)
                {
                    throw new IndexOutOfRangeException();
                }

                return group[index];
            }

            public PacketGroupData<ulong> GetPlayerEntry(int index)
                => GetEntry(this.m_Players, index, this.PlayerCount);
            public PacketGroupData<uint> GetEnemyEntry(int index)
                => GetEntry(this.m_Enemies, index, this.EnemyCount);

            public IEnumerable<PacketGroupData<ulong>> Players
            {
                get
                {
                    for (int index = 0; index < this.PlayerCount; index++)
                    {
                        yield return this.GetPlayerEntry(index);
                    }
                }
            }
            public IEnumerable<PacketGroupData<uint>> Enemies
            {
                get
                {
                    for (int index = 0; index < this.EnemyCount; index++)
                    {
                        yield return this.GetEnemyEntry(index);
                    }
                }
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct PacketGroupData<TGroupID>
            where TGroupID : struct
        {
            public TGroupID ID;
            public float Damage;

            public PacketGroupData(TGroupID id, float damage)
            {
                this.ID = id;
                this.Damage = damage;
            }
        }

        public sealed class CustomData : TriggerData
        {
            public EnemyRestrictions? Enemies { get; set; }
            public DamageRequirements? Damage { get; set; }

            internal bool IsValid(PacketData data)
            {
                if (this.Enemies != null && !this.Enemies.IsValid(data.EnemyID))
                {
                    return false;
                }
                if (this.Damage == null)
                {
                    return true;
                }

                return this.Damage.IsValid(data.DamageTargets);
            }
        }

        public sealed class DamageRequirements
        {
            public bool HasRequirements { get; set; }

            public List<DamageRequirement<EnemyRestrictions>?>? EnemyRequirements { get; set; } = new();
            public List<DamageRequirement<PlayerRestrictions>?>? PlayerRequirements { get; set; } = new();

            internal bool IsValid(PacketGroupList groupData)
            {
                if (!this.HasRequirements)
                {
                    return true;
                }

                if (this.EnemyRequirements != null)
                {
                    foreach (PacketGroupData<uint> enemyInfo in groupData.Enemies)
                    {
                        bool meets = this.EnemyRequirements.Count == 0;
                        foreach (DamageRequirement<EnemyRestrictions>? requirement in this.EnemyRequirements)
                        {
                            if (requirement == null ||
                                requirement.Restrictions == null)
                            {
                                continue;
                            }

                            if (!requirement.Restrictions.IsValid(enemyInfo.ID))
                            {
                                continue;
                            }

                            if (!requirement.Damage.IsValid(enemyInfo.Damage))
                            {
                                continue;
                            }

                            meets = true;
                        }

                        if (!meets)
                        {
                            return false;
                        }
                    }
                }

                if (this.PlayerRequirements != null)
                {
                    foreach (PacketGroupData<ulong> playerInfo in groupData.Players)
                    {
                        SNetwork.SNet_Player? actualPlayer = null;
                        foreach (SNetwork.SNet_Player player in SNetwork.SNet.LobbyPlayers)
                        {
                            if (player.Lookup == playerInfo.ID)
                            {
                                actualPlayer = player;
                                break;
                            }
                        }

                        if (actualPlayer == null)
                        {
                            continue;
                        }

                        bool meets = this.PlayerRequirements.Count == 0;
                        foreach (DamageRequirement<PlayerRestrictions>? requirement in this.PlayerRequirements)
                        {
                            if (requirement == null ||
                                requirement.Restrictions == null)
                            {
                                continue;
                            }

                            if (!requirement.Restrictions.IsValid(actualPlayer))
                            {
                                continue;
                            }

                            if (!requirement.Damage.IsValid(playerInfo.Damage))
                            {
                                continue;
                            }

                            meets = true;
                        }

                        if (!meets)
                        {
                            return false;
                        }
                    }
                }

                return true;
            }
        }

        public sealed class DamageRequirement<TGroup>
        {
            public TGroup? Restrictions { get; set; }
            public MinMaxRestriction<float> Damage { get; set; }
        }

        private static class Patches
        {
            public static bool m_isExplosionAbilityContext;
            public static EnemyAgent? m_enemy;
            public static bool m_isExplodeContext;
            public static List<(IDamageable damageable, float damage)> m_damageTargets = new();

            [HarmonyPatch(typeof(ExplosionExtension), "DoExplode")]
            [HarmonyPrefix]
            [HarmonyWrapSafe]
            public static void ExplodeReceivePrefix(Agent from)
            {
                m_enemy = from.TryCast<EnemyAgent>();
                m_isExplosionAbilityContext = m_enemy != null;
            }
            [HarmonyPatch(typeof(ExplosionExtension), "DoExplode")]
            [HarmonyPostfix]
            [HarmonyWrapSafe]
            public static void ExplodeReceivePostfix()
            {
                m_isExplosionAbilityContext = false;
            }
            [HarmonyPatch(typeof(ExplosionManager), "Internal_TriggerExplosion")]
            [HarmonyPrefix]
            [HarmonyWrapSafe]
            public static void TriggerExplodePrefix()
            {
                m_isExplodeContext = SNetwork.SNet.IsMaster;
            }
            [HarmonyPatch(typeof(ExplosionManager), "Internal_TriggerExplosion")]
            [HarmonyPostfix]
            [HarmonyWrapSafe]
            public static void TriggerExplodePostfix()
            {
                m_isExplodeContext = false;
                if (SNetwork.SNet.IsMaster && m_isExplosionAbilityContext && m_enemy != null)
                {
                    PacketData data = new(m_enemy.EnemyDataID, m_damageTargets);
                    NetworkAPI.InvokeEvent<PacketData>(NETWORK_EVENT_ID, data);
                    OnReceiveTrigger(SNetwork.SNet.Master.Lookup, data);
                }
                m_damageTargets.Clear();
            }
            [HarmonyPatch(typeof(Dam_EnemyDamageBase), nameof(Dam_EnemyDamageBase.ExplosionDamage))]
            [HarmonyPrefix]
            [HarmonyWrapSafe]
            public static void ExplosionDamageEnemy(Dam_EnemyDamageBase __instance, float dam)
            {
                if (m_isExplodeContext)
                {
                    m_damageTargets.Add((__instance.Cast<IDamageable>(), dam));
                }
            }
            [HarmonyPatch(typeof(Dam_PlayerDamageBase), nameof(Dam_PlayerDamageBase.ExplosionDamage))]
            [HarmonyPrefix]
            [HarmonyWrapSafe]
            public static void ExplosionDamagePlayer(Dam_PlayerDamageBase __instance, float dam)
            {
                if (m_isExplodeContext)
                {
                    m_damageTargets.Add((__instance.Cast<IDamageable>(), dam));
                }
            }
        }
    }
}
