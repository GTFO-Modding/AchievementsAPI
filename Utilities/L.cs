using BepInEx.Logging;

namespace AchievementsAPI.Utilities
{
    internal static class L
    {
        private static ManualLogSource Source => MainPlugin.LogSource!;

        public static void Msg(object data)
        {
            Source.LogMessage(data);
        }

        public static void Info(object data)
        {
            Source.LogInfo(data);
        }

        public static void Debug(object data)
        {
            Source.LogDebug(data);
        }

        public static void Warn(object data)
        {
            Source.LogWarning(data);
        }

        public static void Error(object data)
        {
            Source.LogError(data);
        }
    }
}
