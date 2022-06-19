using AchievementsAPI.Triggers;
using System;

namespace AchievementsAPI.Progress
{
    /// <summary>
    /// The progress for an achievement trigger.
    /// </summary>
    public sealed class AchievementTriggerProgress : IAchievementTriggerProgress
    {
        /// <inheritdoc/>
        public int TriggerCount { get; set; }

        /// <summary>
        /// Initializes this progress instance and sets <c>TriggerCount</c>
        /// to 0.
        /// </summary>
        public AchievementTriggerProgress()
        {
            this.TriggerCount = 0;
        }

        /// <summary>
        /// Clones this progress.
        /// </summary>
        /// <returns>A cloned version of this.</returns>
        public AchievementTriggerProgress Clone()
        {
            AchievementTriggerProgress clone = new();
            clone.TriggerCount = this.TriggerCount;
            return clone;
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return HashCode.Combine(this.TriggerCount);
        }

        /// <inheritdoc/>
        public override bool Equals(object? obj)
        {
            if (obj is null)
            {
                return false;
            }

            if (obj is AchievementTriggerProgress otherProgress)
            {
                return this.Equals(otherProgress);
            }

            if (obj is IAchievementTriggerProgress other)
            {
                return ((IEquatable<IAchievementTriggerProgress>)this).Equals(other);
            }

            return false;
        }

        /// <summary>
        /// Returns whether the other trigger progress equals this trigger progress.
        /// </summary>
        /// <param name="other">The other trigger progress.</param>
        /// <returns><see langword="true"/> if the TriggerCount and Data are equal, otherwise
        /// <see langword="false"/></returns>
        public bool Equals(AchievementTriggerProgress? other) 
            => ((IEquatable<IAchievementTriggerProgress>)this).Equals(other);

        IAchievementTriggerProgress IAchievementTriggerProgress.Clone() => this.Clone();

        bool IEquatable<IAchievementTriggerProgress>.Equals(IAchievementTriggerProgress? other)
        {
            return other is not null &&
                other.TriggerCount == this.TriggerCount;
        }

        TriggerProgressData? IAchievementTriggerProgress.Data
        {
            get => null;
            set
            { }
        }
    }

    /// <summary>
    /// The progress for an achievement trigger.
    /// </summary>
    /// <typeparam name="TData">The Progress Data</typeparam>
    public sealed class AchievementTriggerProgress<TData> : IAchievementTriggerProgress
        where TData : TriggerProgressData, new()
    {
        /// <inheritdoc/>
        public int TriggerCount { get; set; }
        /// <summary>
        /// The progress data.
        /// </summary>
        public TData Data { get; set; }

        /// <summary>
        /// Initializes an instance of this progress, by setting
        /// <c>TriggerCount</c> to 0, and setting <c>Data</c>
        /// to a new instance of <typeparamref name="TData"/>.
        /// </summary>
        public AchievementTriggerProgress()
        {
            this.TriggerCount = 0;
            this.Data = new();
        }

        /// <summary>
        /// Clones this progress.
        /// </summary>
        /// <returns>A cloned version of this.</returns>
        public AchievementTriggerProgress<TData> Clone()
        {
            AchievementTriggerProgress<TData> clone = new();
            clone.TriggerCount = this.TriggerCount;
            clone.Data = (TData)this.Data.Clone();
            return clone;
        }

        /// <inheritdoc/>
        public override bool Equals(object? obj)
        {
            if (obj is null)
            {
                return false;
            }

            if (obj is AchievementTriggerProgress<TData> otherProgress)
            {
                return this.Equals(otherProgress);
            }

            if (obj is IAchievementTriggerProgress other)
            {
                return ((IEquatable<IAchievementTriggerProgress>)this).Equals(other);
            }

            return false;
        }

        /// <inheritdoc/>
        public override int GetHashCode() => HashCode.Combine(this.TriggerCount, this.Data);

        /// <inheritdoc/>
        public bool Equals(AchievementTriggerProgress<TData>? other)
        {
            return ((IEquatable<IAchievementTriggerProgress>)this).Equals(other) &&
                this.Data.Equals(other.Data);
        }

        bool IEquatable<IAchievementTriggerProgress>.Equals(IAchievementTriggerProgress? other)
        {
            return other is not null &&
                other.TriggerCount == this.TriggerCount;
        }

        IAchievementTriggerProgress IAchievementTriggerProgress.Clone() => this.Clone();

        TriggerProgressData? IAchievementTriggerProgress.Data
        {
            get => this.Data;
            set
            {
                if (value is not null && value is TData data)
                {
                    this.Data = data;
                }
            }
        }
    }
}
