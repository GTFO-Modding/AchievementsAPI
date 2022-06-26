using AchievementsAPI.Managers;
using AchievementsAPI.Progress;
using AchievementsAPI.Triggers.Attributes;
using AchievementsAPI.Utilities;
using GameData;
using HarmonyLib;
using LevelGeneration;
using SNetwork;
using System.Diagnostics.CodeAnalysis;

namespace AchievementsAPI.Triggers.BuiltIn
{
    [TriggerPatch(typeof(Patches))]
    public sealed class ObjectiveCompleteTrigger : AchievementTrigger<ObjectiveCompleteTrigger.CustomData>
    {
        public const string ID = "CompleteObjective";

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
                        new("triggerType", typeof(TriggerType)),
                        new("layer", typeof(LG_LayerType)))
                };
            }
        }

        private static bool TryGetTriggerData(object?[] data, [NotNullWhen(true)] out TriggerType trigger, [NotNullWhen(true)] out LG_LayerType layer)
        {
            trigger = TriggerType.None;
            layer = LG_LayerType.MainLayer;

            if (data.Length == 0 || data[0] is not TriggerType t)
            {
                return false;
            }
            if (data.Length == 1 || data[1] is not LG_LayerType l)
            {
                return false;
            }

            trigger = t;
            layer = l;
            return true;
        }

        public override bool CanBeTriggered(object?[] data)
        {
            if (!TryGetTriggerData(data, out TriggerType trigger, out LG_LayerType layer))
            {
                return false;
            }

            Il2CppSystem.Collections.Generic.Dictionary<LG_LayerType, WardenObjectiveDataBlock> activeWardenObjectives = WardenObjectiveManager.Current.m_activeWardenObjectives;
            if (!activeWardenObjectives.ContainsKey(layer))
            {
                return false;
            }

            WardenObjectiveDataBlock db = activeWardenObjectives[layer];

            return this.Data.IsValid(db, layer, trigger);
        }

        public override void Trigger(object?[] data, AchievementTriggerProgress progress)
        {
            if (!TryGetTriggerData(data, out TriggerType type, out LG_LayerType _))
            {
                return;
            }

            switch (type)
            {
                case TriggerType.Activate:
                    progress.TriggerCount++;
                    break;
                case TriggerType.ResetFromLevelEnd:
                    progress.TriggerCount = 0;
                    break;
                case TriggerType.DecrementFromCheckpoint:
                    progress.TriggerCount--;
                    break;
            }
        }

        public enum TriggerType
        {
            None,
            Activate,
            DecrementFromCheckpoint,
            ResetFromLevelEnd
        }

        public sealed class CustomData : TriggerData
        {
            public bool ResetOnLevelEnd { get; set; }
            public LG_LayerType ObjectiveLayer { get; set; }
            public WardenObjectiveRestrictions? Objectives { get; set; }

            public bool IsValid(WardenObjectiveDataBlock datablock, LG_LayerType layer, TriggerType type)
            {
                if (this.ObjectiveLayer != layer)
                {
                    return false;
                }

                if (!(this.Objectives?.IsValid(datablock) ?? true))
                {
                    return false;
                }

                switch (type)
                {
                    case TriggerType.ResetFromLevelEnd:
                        return this.ResetOnLevelEnd;
                    default:
                        return true;
                }
            }
        }

        private static class Patches
        {
            public static bool Main { get; set; } = false;
            public static bool Second { get; set; } = false;
            public static bool Third { get; set; } = false;

            [HarmonyPatch(typeof(WardenObjectiveManager), nameof(WardenObjectiveManager.AttemptInteract))]
            [HarmonyPrefix]
            public static void AttemptInteract(pWardenObjectiveInteraction interaction)
            {
                if (interaction.type != eWardenObjectiveInteractionType.SolveWardenObjectiveItem)
                {
                    return;
                }

                bool invoke = false;
                if (!Main && interaction.inLayer == LG_LayerType.MainLayer)
                {
                    Main = true;
                    invoke = true;
                }
                if (!Second && interaction.inLayer == LG_LayerType.SecondaryLayer)
                {
                    Second = true;
                    invoke = true;
                }
                if (!Third && interaction.inLayer == LG_LayerType.ThirdLayer)
                {
                    Third = true;
                    invoke = true;
                }

                if (!invoke)
                {
                    return;
                }

                AchievementManager.ActivateTrigger(ID, TriggerType.Activate, interaction.inLayer);
            }

            [HarmonyPatch(typeof(GS_InLevel), nameof(GS_InLevel.Exit))]
            [HarmonyPrefix]
            public static void OnLevelExit()
            {
                Main = false;
                Second = false;
                Third = false;
            }

            // checkpoint compatibility.
            [HarmonyPatch(typeof(SNet_Capture), nameof(SNet_Capture.OnReceiveBufferCompletion))]
            [HarmonyPostfix]
            public static void OnReceiveBufferCompletion(pBufferCompletion completion)
            {
                if (completion.type != eBufferType.Checkpoint)
                {
                    return;
                }

                // untested, but may work?
                // I don't know if WardenObjectiveManager has updated it's state
                // by the time this patch gets called.
                if (Main && WardenObjectiveManager.CurrentState.main_status != eWardenObjectiveStatus.WardenObjectiveItemSolved)
                {
                    Main = false;
                    AchievementManager.ActivateTrigger(ID, TriggerType.DecrementFromCheckpoint, LG_LayerType.MainLayer);
                }

                if (Second && WardenObjectiveManager.CurrentState.second_status != eWardenObjectiveStatus.WardenObjectiveItemSolved)
                {
                    Second = false;
                    AchievementManager.ActivateTrigger(ID, TriggerType.DecrementFromCheckpoint, LG_LayerType.SecondaryLayer);
                }

                if (Third && WardenObjectiveManager.CurrentState.third_status != eWardenObjectiveStatus.WardenObjectiveItemSolved)
                {
                    Third = false;
                    AchievementManager.ActivateTrigger(ID, TriggerType.DecrementFromCheckpoint, LG_LayerType.ThirdLayer);
                }
            }
        }
    }
}
