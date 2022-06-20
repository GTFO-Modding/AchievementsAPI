using System;

namespace AchievementsAPI.Triggers.Attributes
{
    /// <summary>
    /// Add to Triggers to specify patch classes that will only be added
    /// when the trigger is registered.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public class TriggerPatchAttribute : Attribute
    {
        private readonly Type m_patchType;

        /// <summary>
        /// Add a type to patch when this trigger is registered.
        /// </summary>
        /// <param name="type">The type to patch.</param>
        public TriggerPatchAttribute(Type type)
        {
            this.m_patchType = type;
        }

        /// <summary>
        /// Returns the Type of the patch class.
        /// </summary>
        /// <returns>Type of the patch class.</returns>
        public Type GetPatchType() => this.m_patchType;
    }
}
