using AchievementsAPI.Registries;
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
                foreach (var trigger in this.Definition.Triggers)
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

                    var triggerInfo = this.Progress.GetTriggerInfo(triggerID, increment);
                    if (triggerInfo.Progress.TriggerCount < trigger.Count)
                    {
                        return false;
                    }
                }
                return true;
            }
        }

        internal void ActivateTrigger(string id, object?[] data, ref bool save)
        {
            if (this.Completed)
                return;

            foreach (var condition in this.Definition.Conditions)
            {
                if (!condition.IsMet())
                    return;
            }

            var triggers = this.Definition.Triggers.GetValues(id);
            for (int increment = 0; increment < triggers.Length; increment++)
            {
                var trigger = triggers[increment];
                if (trigger.ConditionOverrides?.AdditionalConditions != null)
                {
                    foreach (var condition in trigger.ConditionOverrides.AdditionalConditions)
                    {
                        if (!condition.IsMet())
                        {
                            return;
                        }
                    }
                }

                var triggerInfo = this.Progress.GetTriggerInfo(id, (uint)increment);
                var progress = triggerInfo.Progress;
                trigger.Trigger(data, ref progress);

                if (triggerInfo.Progress != progress)
                {
                    save = true;
                }

                triggerInfo.Progress = progress;

            }

            if (this.Completed)
            {
                AchievementManager.InvokeOnAchievementUnlocked(this.Definition);
            }
        }
    }
}
