using AchievementsAPI.Progress;
using System;

namespace AchievementsAPI.Triggers
{
    /// <summary>
    /// The class to implement for implementing Achievement Triggers with data and progress data.
    /// </summary>
    /// <typeparam name="TData">The Data this trigger holds.</typeparam>
    /// <typeparam name="TProgressData">The data the progress of this trigger should hold.</typeparam>
    public abstract class AchievementTriggerWithProgress<TData, TProgressData> : AchievementTriggerBase<TData>, IAchievementTrigger<TProgressData>
        where TData : TriggerData, new()
        where TProgressData : TriggerProgressData, new()
    {
        /// <inheritdoc/>
        public sealed override Type? GetProgressDataType() => typeof(TProgressData);

        /// <summary>
        /// Reset the progress to default.
        /// <para>
        /// Default implementation sets <c>TriggerCount</c> to 0, and
        /// sets data to a new instance of <typeparamref name="TProgressData"/>.
        /// </para>
        /// </summary>
        /// <param name="progress">The progress to reset.</param>
        public virtual void ResetProgress(AchievementTriggerProgress<TProgressData> progress)
        {
            progress.TriggerCount = 0;
            progress.Data = new();
        }
        /// <summary>
        /// Triggers this trigger.
        /// </summary>
        /// <param name="data">The data</param>
        /// <param name="progress">The progress that can be modified</param>
        public abstract void Trigger(object?[] data, AchievementTriggerProgress<TProgressData> progress);

        internal sealed override void ResetProgress(IAchievementTriggerProgress progress)
            => this.ResetProgress((AchievementTriggerProgress<TProgressData>)progress);

        internal sealed override void Trigger(object?[] data, IAchievementTriggerProgress progress)
            => this.Trigger(data, (AchievementTriggerProgress<TProgressData>)progress);
    }

    /// <summary>
    /// The class to implement for implementing Achievement Triggers with progress data and no internal data..
    /// </summary>
    /// <typeparam name="TProgressData">The data the progress of this trigger should hold.</typeparam>
    public abstract class AchievementTriggerWithProgress<TProgressData> : AchievementTriggerWithProgress<TriggerData, TProgressData>
        where TProgressData : TriggerProgressData, new()
    { }
}
