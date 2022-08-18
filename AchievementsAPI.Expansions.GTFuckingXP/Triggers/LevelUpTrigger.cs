using AchievementsAPI.Progress;
using AchievementsAPI.Triggers;
using AchievementsAPI.Triggers.Attributes;
using GTFuckingXP.Extensions;
using GTFuckingXP.Information.Level;
using System;

namespace AchievementsAPI.Expansions.GTFuckingXP.Triggers
{
    [TriggerSetupMethod(nameof(SetupCallback))]
    public sealed class LevelUpTrigger : AchievementTrigger
    {
        public const string ID = "GTFuckingXP.LevelUp";

        public override string GetID()
        {
            return ID;
        }

        public override void Trigger(object?[] data, AchievementTriggerProgress progress)
        {
            throw new NotImplementedException();
        }

        public sealed class CustomData : TriggerData
        {

        }

        private static void SetupCallback()
        {
            CacheApiWrapper.AddLvlUpCallback(OnLevelUp);
        }

        private static void OnLevelUp(Level level)
        {
        }
    }
}
