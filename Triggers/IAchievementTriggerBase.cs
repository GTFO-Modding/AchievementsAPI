using AchievementsAPI.Conditions;
using AchievementsAPI.Registries;
using System;

namespace AchievementsAPI.Triggers
{
    /// <summary>
    /// A base achievement trigger.
    /// </summary>
    public interface IAchievementTriggerBase : IRegisterable
    {
        /// <summary>
        /// The activations this trigger requires.
        /// </summary>
        int Count { get; set; }
        /// <summary>
        /// The data associated with this trigger.
        /// </summary>
        TriggerData Data { get; set; }
        /// <summary>
        /// The condition overrides associated with this trigger.
        /// </summary>
        ConditionOverrides ConditionOverrides { get; set; }

        /// <summary>
        /// Gets the type of data this trigger contains.
        /// </summary>
        /// <returns>The type of data this trigger contains.</returns>
        Type GetDataType();
        /// <summary>
        /// Gets the type of progress data this trigger contains.
        /// <para>
        /// Returns <see langword="null"/> if this trigger stores
        /// no progress data.
        /// </para>
        /// </summary>
        /// <returns>The type of progress data or <see langword="null"/>.</returns>
        Type? GetProgressDataType();

        /// <summary>
        /// Set up this achievement trigger.
        /// </summary>
        void Setup();
    }
}
