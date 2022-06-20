namespace AchievementsAPI.Utilities
{
    /// <summary>
    /// Information about a level
    /// </summary>
    public class LevelInfo
    {
        /// <summary>
        /// The Expedition Index
        /// </summary>
        public int ExpeditionIndex { get; set; }
        /// <summary>
        /// The Expedition Tier
        /// </summary>
        public eRundownTier Tier { get; set; }
    }
    
}
