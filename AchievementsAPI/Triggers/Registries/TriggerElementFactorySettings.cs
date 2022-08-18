using AchievementsAPI.Progress;
using Flaff.Collections.Registries;
using System;

namespace AchievementsAPI.Triggers.Registries
{
    /// <summary>
    /// The settings for trigger elements.
    /// </summary>
    public class TriggerElementFactorySettings : RegistryElementFactorySettings<IAchievementTriggerBase>
    {
        /// <summary>
        /// The type of the progress data.
        /// </summary>
        public Type? ProgressDataType { get; }
        /// <summary>
        /// The type of the progress.
        /// </summary>
        public Type ProgressType { get; }
        /// <summary>
        /// Whether or not the trigger has no data.
        /// </summary>
        public bool HasNoData { get; }

        /// <summary>
        /// Creates new Trigger Element descriptor for the given trigger.
        /// </summary>
        /// <param name="trigger">The trigger.</param>
        public TriggerElementFactorySettings(IAchievementTriggerBase trigger) : base(trigger)
        {
            this.ProgressDataType = trigger.GetProgressDataType();
            this.HasNoData = this.ProgressDataType == null || this.ProgressDataType == typeof(TriggerProgressData);

            if (this.HasNoData)
            {
                this.ProgressType = typeof(AchievementTriggerProgress);
            }
            else
            {
                this.ProgressType = typeof(AchievementTriggerProgress<>)
                    .MakeGenericType(this.ProgressDataType!);
            }
        }

        /// <summary>
        /// Creates an instance of the trigger's progress data.
        /// </summary>
        /// <returns>A new progress data instance.</returns>
        public TriggerProgressData CreateProgressDataInstance()
        {
            if (this.HasNoData)
            {
                return new();
            }

            return (TriggerProgressData?)Activator.CreateInstance(this.ProgressDataType!) ?? new();
        }

        internal IAchievementTriggerProgress CreateProgressInstance()
        {
            if (this.HasNoData)
            {
                return new AchievementTriggerProgress();
            }

            IAchievementTriggerProgress result = (IAchievementTriggerProgress?)Activator.CreateInstance(this.ProgressType) ?? throw new InvalidOperationException();

            result.Data = this.CreateProgressDataInstance();

            return result;
        }
    }
}
