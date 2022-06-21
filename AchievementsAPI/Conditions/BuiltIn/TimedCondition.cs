using AchievementsAPI.Managers;
using AchievementsAPI.Triggers;
using System;
using System.Collections.Generic;

namespace AchievementsAPI.Conditions.BuiltIn
{
    public sealed class TimedCondition : AchievementCondition<TimedCondition.CustomData>
    {
        public const string ID = "Timed";

        private readonly AchievementCache m_cache = new();

        public override string GetID()
        {
            return ID;
        }

        public override bool IsMet()
        {
            return true;
        }

        public override void BeforeTriggerActivated(string achievementID, IAchievementTriggerBase trigger, uint triggerIncrement)
        {
            TriggerActivationInfo activationInfo = this.m_cache.GetActivationInfo(achievementID, trigger.GetID(), triggerIncrement);

            if (activationInfo.m_activations > 0)
            {
                if ((DateTime.Now - activationInfo.m_startTime).TotalSeconds >= this.Data.Time)
                {
                    activationInfo.m_activations = 0;
                    AchievementManager.ResetTriggerProgress(achievementID, trigger.GetID(), triggerIncrement);
                }
            }
        }

        public override void AfterTriggerActivated(string achievementID, IAchievementTriggerBase trigger, uint triggerIncrement)
        {
            TriggerActivationInfo info = this.m_cache.GetActivationInfo(achievementID, trigger.GetID(), triggerIncrement);
            if (info.m_activations == 0)
            {
                info.m_startTime = DateTime.Now;
            }
            info.m_activations++;
        }

        private sealed class AchievementCache
        {
            private readonly Dictionary<string, TriggerCache> m_cache = new();

            public void Clear()
            {
                this.m_cache.Clear();
            }

            public TriggerActivationInfo GetActivationInfo(string achievementID, string triggerID, uint increment)
            {
                TriggerActivationInfo info = this.GetTriggerCache(achievementID)
                    .GetActivationInfo(triggerID, increment);

                if (!info.m_setup)
                {
                    AchievementManager.ResetTriggerProgress(achievementID, triggerID, increment);
                    info.m_setup = true;
                }
                return info;
            }

            public TriggerCache GetTriggerCache(string achievementID)
            {
                if (!this.m_cache.TryGetValue(achievementID, out TriggerCache? result))
                {
                    result = new();
                    this.m_cache.Add(achievementID, result);
                }
                return result;
            }
        }

        private sealed class TriggerCache
        {
            private readonly Dictionary<string, Dictionary<uint, TriggerActivationInfo>> m_cache = new();

            public void Clear()
            {
                this.m_cache.Clear();
            }

            private Dictionary<uint, TriggerActivationInfo> GetActivationCache(string triggerID)
            {
                if (!this.m_cache.TryGetValue(triggerID, out Dictionary<uint, TriggerActivationInfo>? result))
                {
                    result = new();
                    this.m_cache.Add(triggerID, result);
                }
                return result;
            }

            public TriggerActivationInfo GetActivationInfo(string triggerID, uint increment)
            {
                Dictionary<uint, TriggerActivationInfo> activationCache = this.GetActivationCache(triggerID);
                if (!activationCache.TryGetValue(increment, out TriggerActivationInfo? result))
                {
                    result = new();
                    activationCache.Add(increment, result);
                }
                return result;
            }
        }

        private sealed class TriggerActivationInfo
        {
            public bool m_setup;
            public int m_activations;
            public DateTime m_startTime;
        }

        public sealed class CustomData : ConditionData
        {
            public double Time { get; set; }
        }
    }
}
