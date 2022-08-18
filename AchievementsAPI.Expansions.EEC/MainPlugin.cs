using BepInEx;
using BepInEx.Unity.IL2CPP;
using AchievementsAPI.Utilities;
using AchievementsAPI.Managers;
using AchievementsAPI.Expansions.EEC.Triggers;

namespace AchievementsAPI.Expansions.EEC
{
    [BepInPlugin(PLUGIN_CONSTANTS.PLUGIN_GUID, PLUGIN_CONSTANTS.PLUGIN_NAME, PLUGIN_CONSTANTS.PLUGIN_VERSION)]
    [BepInProcess("GTFO.exe")]
    [BepInDependency("GTFO.EECustomization", BepInDependency.DependencyFlags.HardDependency)]
    [BepInDependency(AchievementsAPIConstants.GUID, BepInDependency.DependencyFlags.HardDependency)]
    internal sealed class MainPlugin : BasePlugin
    {
        public override void Load()
        {
            RegistryManager.Triggers.Register<EnemyExplodeTrigger>();
        }

    }
}