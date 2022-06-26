using AchievementsAPI.Conditions;
using AchievementsAPI.Conditions.Extensions;
using AchievementsAPI.Managers;
using AchievementsAPI.Progress;
using AchievementsAPI.Registries;
using AchievementsAPI.Triggers;
using AchievementsAPI.Triggers.Extensions;
using AchievementsAPI.Utilities;
using System.Collections.Generic;

namespace AchievementsAPI.Achievements
{
    internal class AchievementInstance
    {
        public AchievementDefinition Definition { get; }
        public AchievementProgress Progress { get; }

        public AchievementInstance(AchievementDefinition definition, AchievementProgress progress)
        {
            this.Definition = definition;
            this.Progress = progress;
        }

        public bool Completed
        {
            get
            {
                Dictionary<string, uint> incrementMap = new();
                foreach (IAchievementTriggerBase? trigger in this.Definition.Triggers)
                {
                    string triggerID = trigger.GetID();
                    if (!incrementMap.TryGetValue(triggerID, out uint increment))
                    {
                        incrementMap.Add(triggerID, increment);
                    }
                    else
                    {
                        incrementMap[triggerID] = ++increment;
                    }

                    AchievementProgress.TriggerInfo? triggerInfo = this.Progress.GetTriggerInfo(triggerID, increment);
                    if (triggerInfo.Progress.TriggerCount < trigger.Count)
                    {
                        return false;
                    }
                }
                return true;
            }
        }

        internal void ResetProgress(string id, uint increment)
        {
            bool save = false;

            this.DoResetProgress(id, increment, ref save);

            if (save)
            {
                AchievementManager.SaveProgress();
            }
        }

        internal void ResetProgress(string id)
        {
            bool save = false;

            this.DoResetProgress(id, ref save);

            if (save)
            {
                AchievementManager.SaveProgress();
            }
        }

        private void DoResetProgress(uint increment, IAchievementTriggerBase[] triggers, ref bool save)
        {
            if (increment >= triggers.Length)
            {
                L.Error($"Attempted to reset trigger with id {triggers[0].GetID()} and increment {increment}, but it is an invalid increment!");
                return;
            }

            var trigger = (_IAchievementTriggerBase)triggers[(int)increment];
            //if (trigger.ConditionOverrides?.AdditionalConditions != null)
            //{
            //    foreach (IAchievementCondition? condition in trigger.ConditionOverrides!.AdditionalConditions)
            //    {
            //        if (!condition.IsMetSafe())
            //        {
            //            return;
            //        }
            //    }
            //}

            AchievementProgress.TriggerInfo triggerInfo = this.Progress.GetTriggerInfo(trigger.GetID(), increment);
            IAchievementTriggerProgress oldProgress = triggerInfo.Progress.Clone();
            trigger.ResetProgressSafe(triggerInfo.Progress);

            if (!triggerInfo.Progress.Equals(oldProgress))
            {
                save = true;
            }
        }

        internal double GetProgress()
        {
            int triggers = 0;
            int totalTriggers = 0;

            foreach (AchievementProgress.TriggerInfo trigger in this.Progress.Triggers)
            {
                triggers += trigger.Progress.TriggerCount;
                totalTriggers += this.Definition.Triggers.GetValues(trigger.ID)[(int)trigger.Increment].Count;
            }

            if (totalTriggers == 0)
            {
                return 1.0;
            }
            else
            {
                return (double)triggers / totalTriggers;
            }
        }

        internal void ForceComplete()
        {
            foreach (AchievementProgress.TriggerInfo trigger in this.Progress.Triggers)
            {
                IAchievementTriggerProgress progress = trigger.Progress;
                progress.TriggerCount = this.Definition.Triggers.GetValues(trigger.ID)[(int)trigger.Increment].Count;
                trigger.Progress = progress;
            }
            AchievementManager.SaveProgress();
        }

        private void DoResetProgress(string id, uint increment, ref bool save)
        {
            IAchievementTriggerBase[] triggers = this.Definition.Triggers.GetValues(id);
            if (triggers.Length == 0)
            {
                L.Error($"Attempted to reset trigger with id {id} and increment {increment}, but no triggers exist with id {id}!");
                return;
            }
            this.DoResetProgress(increment, triggers, ref save);
        }

        private void DoResetProgress(string id, ref bool save)
        {
            IAchievementTriggerBase[] triggers = this.Definition.Triggers.GetValues(id);
            if (triggers.Length == 0)
            {
                L.Warn($"Attempted to reset all triggers with id {id}, but no triggers exist with id {id}!");
                return;
            }

            for (int increment = 0; increment < triggers.Length; increment++)
            {
                this.DoResetProgress((uint)increment, triggers, ref save);
            }
        }

        internal void ResetProgress(bool dontSave = false)
        {
            bool save = false;
            foreach (string? id in this.Definition.Triggers.GetIDS())
            {
                this.DoResetProgress(id, ref save);
            }

            if (!dontSave && save)
            {
                AchievementManager.SaveProgress();
            }
        }

        private bool BaseConditionsMet
        {
            get
            {
                foreach (IAchievementCondition condition in this.Definition.Conditions)
                {
                    if (!condition.IsMetSafe())
                    {
                        return false;
                    }
                }
                return true;
            }
        }

        private bool TriggerMeetsConditions(IAchievementTriggerBase trigger, string achievementID, uint triggerIncrement, bool errorResult = true)
        {
            foreach (IAchievementCondition condition in this.Definition.Conditions)
            {
                if (!condition.IsMetForTriggerSafe(achievementID, trigger, triggerIncrement, errorResult))
                {
                    return false;
                }
            }

            return trigger.MeetsConditions(achievementID, triggerIncrement, errorResult);
        }

        private void InvokeBeforeTriggerActivated(IAchievementTriggerBase trigger, string achievementID, uint triggerIncrement)
        {
            foreach (IAchievementCondition condition in this.Definition.Conditions)
            {
                condition.InvokeBeforeTriggerActivated(achievementID, trigger, triggerIncrement);
            }

            ConditionOverrides overrides = trigger.ConditionOverrides;
            if (overrides == null)
            {
                return;
            }

            foreach (IAchievementCondition condition in overrides.AdditionalConditions)
            {
                condition.InvokeBeforeTriggerActivated(achievementID, trigger, triggerIncrement);
            }
        }

        private void InvokeAfterTriggerActivated(IAchievementTriggerBase trigger, string achievementID, uint triggerIncrement)
        {
            foreach (IAchievementCondition condition in this.Definition.Conditions)
            {
                condition.InvokeAfterTriggerActivated(achievementID, trigger, triggerIncrement);
            }

            ConditionOverrides overrides = trigger.ConditionOverrides;
            if (overrides == null)
            {
                return;
            }

            foreach (IAchievementCondition condition in overrides.AdditionalConditions)
            {
                condition.InvokeAfterTriggerActivated(achievementID, trigger, triggerIncrement);
            }
        }

        internal void ActivateTrigger(string id, object?[] data, ref bool save)
        {
            if (this.Completed || !this.BaseConditionsMet)
            {
                return;
            }

            IAchievementTriggerBase[]? triggers = this.Definition.Triggers.GetValues(id);
            for (int increment = 0; increment < triggers.Length; increment++)
            {
                var trigger = (_IAchievementTriggerBase)triggers[increment];

                if (!this.TriggerMeetsConditions(trigger, this.Definition.ID, (uint)increment))
                {
                    continue;
                }

                if (!trigger.CanBeTriggeredSafe(data))
                {
                    continue;
                }

                this.InvokeBeforeTriggerActivated(trigger, this.Definition.ID, (uint)increment);

                AchievementProgress.TriggerInfo triggerInfo = this.Progress.GetTriggerInfo(id, (uint)increment);
                IAchievementTriggerProgress oldProgress = triggerInfo.Progress.Clone();
                trigger.TriggerSafe(data, triggerInfo.Progress);

                this.InvokeAfterTriggerActivated(trigger, this.Definition.ID, (uint)increment);

                if (!triggerInfo.Progress.Equals(oldProgress))
                {
                    save = true;
                }
            }

            if (this.Completed)
            {
                AchievementManager.InvokeOnAchievementUnlocked(this.Definition);
            }
        }
    }
}
