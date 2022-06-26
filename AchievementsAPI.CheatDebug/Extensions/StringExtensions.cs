namespace AchievementsAPI.CheatDebug.Extensions
{
    public static class StringExtensions
    {
        public static string ToMenuID(this string str)
        {
            return MENUS_FULL_PREFIX + '.' + str;
        }
        public static string ToCheatID(this string str)
        {
            return CHEATS_FULL_PREFIX + '.' + str;
        }

        private const string GAME_NAME = "gtfo";
        private const string SUBCATEGORY_ID = "achievementsapi";
        private const string MENUS_PREFIX = $"{GAME_NAME}.menus";
        private const string CHEATS_PREFIX = $"{GAME_NAME}.cheats";
        private const string MENUS_FULL_PREFIX = $"{MENUS_PREFIX}.{SUBCATEGORY_ID}";
        private const string CHEATS_FULL_PREFIX = $"{CHEATS_PREFIX}.{SUBCATEGORY_ID}";
    }
}
