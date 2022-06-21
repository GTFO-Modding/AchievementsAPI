using AchievementsAPI.Conditions;
using AchievementsAPI.Conditions.BuiltIn;
using AchievementsAPI.Managers;
using AchievementsAPI.Triggers;
using AchievementsAPI.Triggers.BuiltIn;

namespace AchievementsAPI
{
    internal partial class MainPlugin
    {
        private void RegisterBuiltIns()
        {
            static void RegisterCondition<T>()
                where T : IAchievementCondition, new()
            {
                RegistryManager.Conditions.Register<T>();
            }
            static void RegisterTrigger<T>()
                where T : IAchievementTriggerBase, new()
            {
                RegistryManager.Triggers.Register<T>();
            }

            RegisterCondition<LevelRestrictionsCondition>();
            RegisterCondition<InBioscanCondition>();
            RegisterCondition<IsCrouchedCondition>();
            RegisterCondition<IsDownedCondition>();

            RegisterTrigger<EnemyBiotrackedTrigger>();
            RegisterTrigger<DownedTrigger>();
            RegisterTrigger<JumpTrigger>();
            RegisterTrigger<ExpeditionFailTrigger>();
            RegisterTrigger<ExpeditionSuccessTrigger>();
        }
    }
}
