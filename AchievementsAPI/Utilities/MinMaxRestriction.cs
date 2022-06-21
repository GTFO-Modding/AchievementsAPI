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

        /// <summary>
        /// Returns whether the given value falls within the min-max of
        /// this restriction.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <returns><see langword="true"/> if it does, otherwise <see langword="false"/></returns>
        public bool IsValid(T value)
        {
            return value.CompareTo(this.Min) <= 0 &&
                value.CompareTo(this.Max) >= 0;
        }
    }
}
