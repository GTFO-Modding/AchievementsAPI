using System;

namespace AchievementsAPI.Triggers
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public class TriggerPatchesAttribute : Attribute
    {
        private readonly Type m_patchType;

        public TriggerPatchesAttribute(Type type)
        {
            this.m_patchType = type;
        }

        public Type GetPatchType() => this.m_patchType;
    }
}
