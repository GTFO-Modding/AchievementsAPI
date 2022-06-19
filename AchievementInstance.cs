using AchievementsAPI.Progress;
using AchievementsAPI.Registries;
using AchievementsAPI.Triggers;
using System.Collections.Generic;

namespace AchievementsAPI
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

        internal void ResetProgress(string id)
        {
            bool save = false;

            this.DoResetProgress(id, ref save);

            if (save)
            {
                AchievementManager.SaveProgress();
            }
        }

        private void DoResetProgress(string id, ref bool save)
        {
            IAchievementTriggerBase[]? triggers = this.Definition.Triggers.GetValues(id);
            for (int increment = 0; increment < triggers.Length; increment++)
            {
                var trigger = (_IAchievementTriggerBase)triggers[increment];
                if (trigger.ConditionOverrides?.AdditionalConditions != null)
                {
                    foreach (Conditions.IAchievementCondition? condition in trigger.ConditionOverrides.AdditionalConditions)
                    {
                        if (!condition.IsMet())
                        {
                            return;
                        }
                    }
                }

                AchievementProgress.TriggerInfo? triggerInfo = this.Progress.GetTriggerInfo(id, (uint)increment);
                IAchievementTriggerProgress? oldProgress = triggerInfo.Progress.Clone();
                trigger.ResetProgress(triggerInfo.Progress);

                if (!triggerInfo.Progress.Equals(oldProgress))
                {
                    save = true;
                }

            }
        }

        internal void ResetProgress()
        {
            bool save = false;
            foreach (string? id in this.Definition.Triggers.GetIDS())
            {
                this.DoResetProgress(id, ref save);
            }

            if (save)
            {
                AchievementManager.SaveProgress();
            }
        }

        internal void ActivateTrigger(string id, object?[] data, ref bool save)
        {
            if (this.Completed)
            {
                return;
            }

            foreach (Conditions.IAchievementCondition? condition in this.Definition.Conditions)
            {
                if (!condition.IsMet())
                {
                    return;
                }
            }

            IAchievementTriggerBase[]? triggers = this.Definition.Triggers.GetValues(id);
            for (int increment = 0; increment < triggers.Length; increment++)
            {
                var trigger = (_IAchievementTriggerBase)triggers[increment];
                if (trigger.ConditionOverrides?.AdditionalConditions != null)
                {
                    foreach (Conditions.IAchievementCondition? condition in trigger.ConditionOverrides.AdditionalConditions)
                    {
                        if (!condition.IsMet())
                        {
                            return;
                        }
                    }
                }

                AchievementProgress.TriggerInfo? triggerInfo = this.Progress.GetTriggerInfo(id, (uint)increment);
                IAchievementTriggerProgress? oldProgress = triggerInfo.Progress.Clone();
                trigger.Trigger(data, triggerInfo.Progress);

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
