using AchievementsAPI.Triggers;
using System;

namespace AchievementsAPI.Conditions
{
    /// <summary>
    /// Base implementation of an achievement condition.
    /// </summary>
    /// <typeparam name="TData">The data this condition holds.</typeparam>
    public abstract class AchievementCondition<TData> : IAchievementCondition
        where TData : ConditionData, new()
    {
        private bool m_isSetup;

        /// <summary>
        /// The Data of this condition
        /// </summary>
        public TData Data { get; set; }

        /// <summary>
        /// Initializes this condition.
        /// </summary>
        protected AchievementCondition()
        {
            this.Data = new();
        }

        /// <inheritdoc/>
        public void Setup()
        {
            if (this.m_isSetup)
            {
                return;
            }

            this.DoSetup();
            this.m_isSetup = true;
        }

        /// <inheritdoc cref="IAchievementCondition.Setup"/>
        protected virtual void DoSetup()
        { }

        /// <inheritdoc/>
        public abstract bool IsMet();

        /// <inheritdoc/>
        public virtual bool IsMetForTrigger(IAchievementTriggerBase trigger)
        {
            return this.IsMet();
        }

        /// <inheritdoc/>
        public virtual void OnTriggerActivated(IAchievementTriggerBase trigger)
        { }

        /// <inheritdoc/>
        public abstract string GetID();
        Type IAchievementCondition.GetDataType() => typeof(TData);
        ConditionData IAchievementCondition.Data
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
    }
    /// <summary>
    /// Base Class all achievement conditions derive from. This condition will hold no data.
    /// </summary>
    public abstract class AchievementCondition : AchievementCondition<ConditionData>
    { }
}
