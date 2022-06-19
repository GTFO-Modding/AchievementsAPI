namespace AchievementsAPI
{
    /// <summary>
    /// The sound that plays when an achievement is achieved.
    /// </summary>
    public class AchievementSound
    {
        /// <summary>
        /// Whether or not a sound is played in the first place.
        /// </summary>
        public bool Enabled { get; set; }
        /// <summary>
        /// The Sound Event ID of the sound to play. Set to 0 to use default.
        /// </summary>
        public uint SoundID { get; set; }
    }
}
