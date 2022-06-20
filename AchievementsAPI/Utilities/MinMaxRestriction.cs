namespace AchievementsAPI.Utilities
{
    /// <summary>
    /// Restrictions for a min max value.
    /// </summary>
    /// <typeparam name="T">The type of value.</typeparam>
    public struct MinMaxRestriction<T>
        where T : struct
    {
        /// <summary>
        /// The minimum value
        /// </summary>
        public T Min { get; set; }
        /// <summary>
        /// The maximum value.
        /// </summary>
        public T Max { get; set; }

        /// <summary>
        /// Initializes this restriction with the provided min max values.
        /// </summary>
        /// <param name="min">The minimum value</param>
        /// <param name="max">The maximum value</param>
        public MinMaxRestriction(T min, T max)
        {
            this.Min = min;
            this.Max = max;
        }
    }
}
