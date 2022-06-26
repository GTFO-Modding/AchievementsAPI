using System;
using System.Collections;
using System.Collections.Generic;

namespace AchievementsAPI.Triggers
{
    /// <summary>
    /// A list of parameters that if satisfied will successfully activate the trigger.
    /// </summary>
    public sealed class TriggerParameterList : IEnumerable<TriggerParameterDefinition>
    {
        private readonly TriggerParameterDefinition[] m_parameters;

        /// <summary>
        /// Initializes this parameter list with no parameters.
        /// </summary>
        public TriggerParameterList()
        {
            this.m_parameters = Array.Empty<TriggerParameterDefinition>();
        }

        /// <summary>
        /// Initializes this parameter list with the given parameters.
        /// </summary>
        /// <param name="parameters">The parameters of this list.</param>
        public TriggerParameterList(params TriggerParameterDefinition[] parameters)
        {
            List<TriggerParameterDefinition> parameterList = new();
            foreach (TriggerParameterDefinition parameter in parameters)
            {
                if (parameter == null)
                {
                    continue;
                }

                parameterList.Add(parameter);
            }

            this.m_parameters = parameterList.ToArray();
        }

        /// <summary>
        /// The number of parameters in this list.
        /// </summary>
        public int Length => this.m_parameters.Length;

        /// <summary>
        /// Gets the parameter at the specific index, throwing
        /// an <see cref="IndexOutOfRangeException"/> if it's
        /// invalid.
        /// </summary>
        /// <param name="index">The index of the parameter.</param>
        /// <returns>The parameter at the specified index.</returns>
        /// <exception cref="IndexOutOfRangeException">Thrown if
        /// <paramref name="index"/> is less than 0, or if
        /// <paramref name="index"/> is greater than or equal to
        /// <see cref="Length">this.Length</see></exception>
        public TriggerParameterDefinition this[int index]
        {
            get
            {
                if (index < 0 || index >= this.Length)
                {
                    throw new IndexOutOfRangeException();
                }

                return this.m_parameters[index];
            }
        }

        /// <inheritdoc/>
        public IEnumerator<TriggerParameterDefinition> GetEnumerator()
        {
            int index = 0;
            while (index < this.Length)
            {
                yield return this[index];
            }
        }
        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
    }
}
