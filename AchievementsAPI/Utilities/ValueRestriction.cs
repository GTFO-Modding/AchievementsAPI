namespace AchievementsAPI.Utilities
{
    /// <summary>
    /// A restriction for a specific value.
    /// </summary>
    /// <typeparam name="T">The type of value</typeparam>
    public struct ValueRestriction<T>
        where T : struct
    {
        /// <summary>
        /// Whether or not this restriction is enabled.
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// The value of this restriction
        /// </summary>
        public T Value { get; set; }

        /// <summary>
        /// Initializes this restriction as an enabled value restriction
        /// with value <paramref name="value"/>.
        /// </summary>
        /// <param name="value">The value of this restriction</param>
        public ValueRestriction(T value)
        {
            this.Enabled = true;
            this.Value = value;
        }
    }
}
