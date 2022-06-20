using System;
using System.Collections;
using System.Collections.Generic;

namespace AchievementsAPI.Registries
{
    /// <summary>
    /// An implementation of a registry list.
    /// </summary>
    /// <typeparam name="TElement">The elements this list holds.</typeparam>
    public class RegistryList<TElement> : IRegistryList<TElement>
        where TElement : IRegisterable
    {
        private readonly List<TElement> m_elements;

        /// <summary>
        /// Initializes this list.
        /// </summary>
        public RegistryList()
        {
            this.m_elements = new();
        }

        /// <inheritdoc/>
        public TElement this[int index]
        {
            get => this.m_elements[index];
            set => this.m_elements[index] = value;
        }

        /// <inheritdoc/>
        public int Count => this.m_elements.Count;
        /// <inheritdoc/>
        public bool IsReadOnly => false;

        /// <inheritdoc/>
        public void Add(TElement item)
        {
            if (item is null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            this.m_elements.Add(item);
        }

        /// <inheritdoc/>
        public void Clear()
        {
            this.m_elements.Clear();
        }

        /// <inheritdoc/>
        public bool Contains(TElement item)
        {
            if (item is null)
            {
                return false;
            }

            return this.m_elements.Contains(item);
        }

        /// <inheritdoc/>
        public void CopyTo(TElement[] array, int arrayIndex)
        {
            if (array is null)
            {
                throw new ArgumentNullException(nameof(array));
            }

            if (arrayIndex + this.Count >= array.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(arrayIndex));
            }

            for (int index = 0; index < this.Count; index++)
            {
                array[arrayIndex++] = this[index];
            }
        }

        /// <inheritdoc/>
        public IEnumerator<TElement> GetEnumerator() => this.m_elements.GetEnumerator();

        /// <inheritdoc/>
        public int IndexOf(TElement item)
        {
            if (item is null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            return this.m_elements.IndexOf(item);
        }

        /// <inheritdoc/>
        public void Insert(int index, TElement item)
        {
            if (item is null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            this.m_elements.Insert(index, item);
        }

        /// <inheritdoc/>
        public bool Remove(TElement item)
        {
            if (item is null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            return this.m_elements.Remove(item);
        }

        /// <inheritdoc/>
        public void RemoveAt(int index)
        {
            this.m_elements.RemoveAt(index);
        }

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

        /// <inheritdoc/>
        public TElement this[string id]
        {
            get
            {
                int index = this.IndexOfID(id);
                if (index == -1)
                {
                    throw new ArgumentOutOfRangeException(nameof(id), $"There is no element with id '{id}'");
                }

                return this[index];
            }
            set
            {
                if (value is null)
                {
                    throw new ArgumentNullException(nameof(value));
                }

                int index = this.IndexOfID(id);
                if (index == -1)
                {
                    this.Add(value);
                }
                else
                {
                    this[index] = value;
                }
            }
        }
    }
}
