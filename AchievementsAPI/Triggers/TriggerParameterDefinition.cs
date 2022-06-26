using System;

namespace AchievementsAPI.Triggers
{
    /// <summary>
    /// A definition of a parameter for achievement triggers.
    /// </summary>
    public sealed class TriggerParameterDefinition
    {
        /// <summary>
        /// The name of this parameter
        /// </summary>
        public readonly string name;
        /// <summary>
        /// The type of this parameter
        /// </summary>
        public readonly Type type;

        /// <summary>
        /// Initializes this definition with the given name and type.
        /// </summary>
        /// <param name="name">The name of the parameter</param>
        /// <param name="type">The type of the parameter.</param>
        /// <exception cref="ArgumentNullException">Thrown if
        /// <see cref="string.IsNullOrWhiteSpace(string?)"/> returns
        /// <see langword="true"/> for <paramref name="name"/>,
        /// or <paramref name="type"/> is <see langword="null"/>.</exception>
        public TriggerParameterDefinition(string name, Type type)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            this.name = name;
            this.type = type ?? throw new ArgumentNullException(nameof(type));
        }
    }
}
