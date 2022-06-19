using System;

namespace AchievementsAPI.Conditions
{
    public abstract class AchievementCondition<TData> : IAchievementCondition
        where TData : ConditionData, new()
    {
        private bool m_isSetup;

        public TData Data { get; set; }

        protected AchievementCondition()
        {
            this.Data = new();
        }

        public void Setup()
        {
            if (this.m_isSetup) return;
            this.DoSetup();
            this.m_isSetup = true;
        }

        protected virtual void DoSetup()
        { }

        public abstract bool IsMet();

        public abstract string GetID();
        Type IAchievementCondition.GetDataType() => typeof(TData);
        ConditionData IAchievementCondition.Data
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
    public abstract class AchievementCondition : AchievementCondition<ConditionData>
    { }
}
