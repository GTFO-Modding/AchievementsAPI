using AchievementsAPI.Triggers;
using System;

namespace AchievementsAPI.Registries
{
    /// <summary>
    /// The registry for Achievement Triggers.
    /// </summary>
    public class TriggerRegistry : RegistryOfFactory<IAchievementTriggerBase, TriggerElementFactorySettings>
    {
        /// <inheritdoc/>
        protected sealed override TriggerElementFactorySettings CreateFactoryInfo(IAchievementTriggerBase element)
            => new(element);

        /// <summary>
        /// Get the <see cref="Type"/> of the Progress Data
        /// for the trigger with id <paramref name="id"/>.
        /// </summary>
        /// <param name="id">The ID of the trigger.</param>
        /// <returns>The type, or <see langword="null"/> if the trigger
        /// doesn't store custom progress data.</returns>
        public Type? GetProgressDataType(string id)
        {
            if (this.TryGetEntry(id, out var entry))
            {
                return entry.ProgressDataType;
            }
            return null;
        }
    }
}
