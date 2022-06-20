namespace AchievementsAPI.Triggers
{
    /// <summary>
    /// Basic progress data for a trigger.
    /// </summary>
    public class TriggerProgressData
    {
        /// <summary>
        /// Whether or not two datas or equal. Overriding
        /// classes should override this method.
        /// </summary>
        /// <param name="other">The other data.</param>
        /// <returns><see langword="true"/> if they
        /// are equal, otherwise <see langword="false"/></returns>
        public virtual bool Equals(TriggerProgressData? other)
        {
            return this.Equals((object?)other);
        }

        /// <summary>
        /// Clones this data. Overriding classes should override this
        /// method if <see cref="object.MemberwiseClone"/> won't create
        /// a full deep clone of this data.
        /// </summary>
        /// <returns>A deep copy of this data.</returns>
        public virtual TriggerProgressData Clone()
        {
            return (TriggerProgressData)this.MemberwiseClone();
        }
    }
}
