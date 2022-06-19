using AchievementsAPI.Conditions.BuiltIn;
using AchievementsAPI.Triggers.BuiltIn;

namespace AchievementsAPI
{
    internal partial class MainPlugin
    {
        private void RegisterBuiltIns()
        {
            AchievementManager.Registry.Conditions.Register<LevelRestrictionsCondition>();
            AchievementManager.Registry.Conditions.Register<InBioscanCondition>();
            AchievementManager.Registry.Conditions.Register<IsDownedCondition>();

            AchievementManager.Registry.Triggers.Register<EnemyBiotrackedTrigger>();
            AchievementManager.Registry.Triggers.Register<JumpTrigger>();
            AchievementManager.Registry.Triggers.Register<ExpeditionFailTrigger>();
        }
    }
}
