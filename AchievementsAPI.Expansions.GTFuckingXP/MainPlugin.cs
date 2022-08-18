using AchievementsAPI.Utilities;
using BepInEx;
using BepInEx.Unity.IL2CPP;
using GTFuckingXP;
using GTFuckingXP.Extensions;

namespace AchievementsAPI.Expansions.GTFuckingXP
{
    [BepInPlugin(PLUGIN_CONSTANTS.PLUGIN_GUID, PLUGIN_CONSTANTS.PLUGIN_NAME, PLUGIN_CONSTANTS.PLUGIN_VERSION)]
    [BepInProcess("GTFO.exe")]
    [BepInDependency(BepInExLoader.GUID, BepInDependency.DependencyFlags.HardDependency)]
    [BepInDependency(AchievementsAPIConstants.GUID, BepInDependency.DependencyFlags.HardDependency)]
    internal sealed class MainPlugin : BasePlugin
    {
        public override void Load()
        {
        }
    }
}