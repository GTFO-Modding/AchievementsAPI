using AchievementsAPI.Conditions;
using System;

namespace AchievementsAPI.Triggers
{
    public abstract class AchievementTrigger<TData> : IAchievementTrigger
        where TData : TriggerData, new()
    {
        private bool m_isSetup;

        public int Count { get; set; }
        public ConditionOverrides ConditionOverrides { get; set; }
        public TData Data { get; set; }

        protected AchievementTrigger()
        {
            this.Data = new();
            this.ConditionOverrides = new();
        }

        public void Setup()
        {
            if (this.m_isSetup) return;

            this.DoSetup();
            this.m_isSetup = true;

            foreach (var condition in this.ConditionOverrides.AdditionalConditions)
            {
                condition.Setup();
            }
        }

        protected virtual void DoSetup()
        { }

        public abstract void Trigger(object?[] data, ref AchievementTriggerProgress progress);

        public abstract string GetID();
        Type IAchievementTrigger.GetDataType() => typeof(TData);
        TriggerData IAchievementTrigger.Data
        {
            get
            {
                if (this.Data is null)
                    this.Data = new();
                return this.Data;
            }
            set
            {
                if (value is TData data)
                {
                    this.Data = data;
                }
                else
                {
                    throw new InvalidCastException($"Cannot convert type {value?.GetType()?.Name ?? "null"} to {typeof(TData).Name}");
                }
            }
        }
    }

    public abstract class AchievementTrigger : AchievementTrigger<TriggerData>
    { }
}
