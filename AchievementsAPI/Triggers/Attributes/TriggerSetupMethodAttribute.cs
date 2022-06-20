using System;

namespace AchievementsAPI.Triggers.Attributes
{
    /// <summary>
    /// Attribute added to triggers to signify a setup method.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public sealed class TriggerSetupMethodAttribute : Attribute
    {
        private readonly string m_methodName;

        /// <summary>
        /// Initializes this attribute to call the method name provided for
        /// the trigger this attribute is attached to setup.
        /// </summary>
        /// <param name="methodName">The method name.</param>
        public TriggerSetupMethodAttribute(string methodName)
        {
            this.m_methodName = methodName;
        }

        /// <summary>
        /// Returns the name of the method this attribute
        /// will call to set up the trigger.
        /// </summary>
        /// <returns>The name of the method.</returns>
        public string GetMethodName()
        {
            return this.m_methodName;
        }
    }
}
