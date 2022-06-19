using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AchievementsAPI.Registries
{
    public sealed class RegistryHandler
    {
        public ConditionRegistry Conditions { get; } = new();
        public TriggerRegistry Triggers { get; } = new();
        public AchievementRegistry Achievements { get; } = new();
    }
}
