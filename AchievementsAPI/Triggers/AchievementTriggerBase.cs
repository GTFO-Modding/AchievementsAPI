using AchievementsAPI.Conditions;
using AchievementsAPI.Progress;
using System;

namespace AchievementsAPI.Triggers
{
    /// <summary>
    /// A base class for triggers.
    /// </summary>
    /// <typeparam name="TData">The Data this trigger holds.</typeparam>
    public abstract class AchievementTriggerBase<TData> : _IAchievementTriggerBase
        where TData : TriggerData, new()
    {
        private bool m_isSetup;

        /// <inheritdoc/>
        public int Count { get; set; }
        /// <inheritdoc/>
        public ConditionOverrides ConditionOverrides { get; set; }
        /// <summary>
        /// The Data of this trigger.
        /// </summary>
        public TData Data { get; set; }

        /// <summary>
        /// Initializes this trigger with <c>Data</c> being set
        /// to a new instance of <typeparamref name="TData"/>, and
        /// <c>ConditionOverrides</c> being set to a new instance
        /// of <see cref="Conditions.ConditionOverrides"/>
        /// </summary>
        protected AchievementTriggerBase()
        {
            this.Data = new();
            this.ConditionOverrides = new();
        }

        /// <inheritdoc cref="IAchievementTriggerBase.TriggerParameters"/>
        protected virtual TriggerParameterList[]? Parameters => null; 

        /// <inheritdoc/>
        public void Setup()
        {
            if (this.m_isSetup)
            {
                return;
            }

            this.DoSetup();
            this.m_isSetup = true;

            foreach (IAchievementCondition? condition in this.ConditionOverrides.AdditionalConditions)
            {
                condition.Setup();
            }
        }

        /// <inheritdoc/>
        public virtual bool CanBeTriggered(object?[] data)
        {
            return true;
        }

        /// <inheritdoc cref="IAchievementTriggerBase.Setup"/>
        protected virtual void DoSetup()
        { }

        internal abstract void ResetProgress(IAchievementTriggerProgress progress);

        internal abstract void Trigger(object?[] data, IAchievementTriggerProgress progress);

        /// <inheritdoc/>
        public abstract string GetID();
        /// <inheritdoc/>
        public abstract Type? GetProgressDataType();

        void _IAchievementTriggerBase.ResetProgress(IAchievementTriggerProgress progress)
            => this.ResetProgress(progress);
        void _IAchievementTriggerBase.Trigger(object?[] data, IAchievementTriggerProgress progress)
            => this.Trigger(data, progress);

        Type IAchievementTriggerBase.GetDataType() => typeof(TData);
        TriggerData IAchievementTriggerBase.Data
        {
            get
            {
                if (this.Data is null)
                {
                    this.Data = new();
                }

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
        TriggerParameterList[]? IAchievementTriggerBase.TriggerParameters => this.Parameters;
    }
}
