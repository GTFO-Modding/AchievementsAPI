using System;

namespace AchievementsAPI.Utilities
{
    /// <summary>
    /// Restrictions for a min max value.
    /// </summary>
    /// <typeparam name="T">The type of value.</typeparam>
    public struct MinMaxRestriction<T>
        where T : struct, IComparable<T>
    {
        /// <summary>
        /// The minimum value
        /// </summary>
        public ValueRestriction<T> Min { get; set; }
        /// <summary>
        /// The maximum value.
        /// </summary>
        public ValueRestriction<T> Max { get; set; }

        /// <summary>
        /// Initializes this restriction with the provided min max values.
        /// </summary>
        /// <param name="min">The minimum value</param>
        /// <param name="max">The maximum value</param>
        public MinMaxRestriction(T min, T max)
        {
            this.Min = new(min);
            this.Max = new(max);
        }

        /// <summary>
        /// Returns whether the given value falls within the min-max of
        /// this restriction.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <returns><see langword="true"/> if it does, otherwise <see langword="false"/></returns>
        public bool IsValid(T value)
        {
            return (!this.Min.Enabled || value.CompareTo(this.Min.Value) <= 0) &&
                (!this.Max.Enabled || value.CompareTo(this.Max.Value) >= 0);
        }
    }
}
