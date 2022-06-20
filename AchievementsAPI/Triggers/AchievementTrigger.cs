using AchievementsAPI.Progress;
using System;

namespace AchievementsAPI.Triggers
{
    /// <summary>
    /// The base class for implementing an achievement trigger with the specified data.
    /// </summary>
    /// <typeparam name="TData">The data for this trigger.</typeparam>
    public abstract class AchievementTrigger<TData> : AchievementTriggerBase<TData>
        where TData : TriggerData, new()
    {
        /// <inheritdoc/>
        public sealed override Type? GetProgressDataType() => null;

        /// <summary>
        /// Reset the progress to default.
        /// <para>
        /// Default implementation sets <c>TriggerCount</c> to 0.
        /// </para>
        /// </summary>
        /// <param name="progress">The progress to reset.</param>
        public virtual void ResetProgress(AchievementTriggerProgress progress)
        {
            progress.TriggerCount = 0;
        }
        /// <summary>
        /// Triggers this trigger.
        /// </summary>
        /// <param name="data">The data</param>
        /// <param name="progress">The progress that can be modified</param>
        public abstract void Trigger(object?[] data, AchievementTriggerProgress progress);

        internal sealed override void ResetProgress(IAchievementTriggerProgress progress) 
            => this.ResetProgress((AchievementTriggerProgress)progress);

        internal sealed override void Trigger(object?[] data, IAchievementTriggerProgress progress) 
            => this.Trigger(data, (AchievementTriggerProgress)progress);
    }

    /// <summary>
    /// The base class for implementing an achievement trigger with no data.
    /// </summary>
    public abstract class AchievementTrigger : AchievementTrigger<TriggerData>
    { }
}
