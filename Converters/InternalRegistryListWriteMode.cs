namespace AchievementsAPI.Converters
{
    /// <summary>
    /// The write mode of an internal registry list.
    /// </summary>
    public enum InternalRegistryListWriteMode : byte
    {
        /// <summary>
        /// Write as a list
        /// </summary>
        List = 0,
        /// <summary>
        /// Write as a map or object
        /// </summary>
        Map = 1
    }
}
