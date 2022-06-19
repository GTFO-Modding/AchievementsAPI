using System;
using System.Collections.Generic;

namespace AchievementsAPI
{
    public class AchievementProgress
    {
        public List<TriggerInfo> Triggers { get; set; } = new();

        public TriggerInfo GetTriggerInfo(string id, uint increment = 0)
        {
            if (this.Triggers == null)
            {
                this.Triggers = new();
                TriggerInfo info = new(id, increment);
                this.Triggers.Add(info);

                return info;
            }

            foreach (TriggerInfo info in this.Triggers)
            {
                if (info.ID == id && info.Increment == increment)
                {
                    return info;
                }
            }

            TriggerInfo newInfo = new(id, increment);
            this.Triggers.Add(newInfo);
            return newInfo;
        }

        public sealed class TriggerInfo
        {
            public string ID { get; set; }
            public uint Increment { get; set; }
            public AchievementTriggerProgress Progress { get; set; }

            public TriggerInfo()
            {
                this.ID = null!;
            }

            public TriggerInfo(string id, uint increment)
            {
                if (id == null)
                    throw new ArgumentNullException(nameof(id));

                this.ID = id;
                this.Increment = increment;
            }
            public TriggerInfo(string id, uint increment, AchievementTriggerProgress progress) : this(id, increment)
            {
                this.Progress = progress;
            }
        }
    }
}
