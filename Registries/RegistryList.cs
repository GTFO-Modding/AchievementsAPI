﻿using System;
using System.Collections;
using System.Collections.Generic;

namespace AchievementsAPI.Registries
{
    public class RegistryList<TElement> : IRegistryList<TElement>
        where TElement : IRegisterable
    {
        private readonly List<TElement> m_elements;

        public RegistryList()
        {
            this.m_elements = new();
        }

        public TElement this[int index]
        {
            get => this.m_elements[index];
            set => this.m_elements[index] = value;
        }

        public int Count => this.m_elements.Count;
        public bool IsReadOnly => false;

        public void Add(TElement item)
        {
            if (item is null)
                throw new ArgumentNullException(nameof(item));

            this.m_elements.Add(item);
        }

        public void Clear()
        {
            this.m_elements.Clear();
        }

        public bool Contains(TElement item)
        {
            if (item is null)
                return false;

            return this.m_elements.Contains(item);
        }

        public void CopyTo(TElement[] array, int arrayIndex)
        {
            if (array is null)
                throw new ArgumentNullException(nameof(array));

            if (arrayIndex + this.Count >= array.Length)
                throw new ArgumentOutOfRangeException(nameof(arrayIndex));

            for (int index = 0; index < this.Count; index++)
            {
                array[arrayIndex++] = this[index];
            }
        }

        public IEnumerator<TElement> GetEnumerator() => this.m_elements.GetEnumerator();

        public int IndexOf(TElement item)
        {
            if (item is null)
                throw new ArgumentNullException(nameof(item));

            return this.m_elements.IndexOf(item);
        }

        public void Insert(int index, TElement item)
        {
            if (item is null)
                throw new ArgumentNullException(nameof(item));

            this.m_elements.Insert(index, item);
        }

        public bool Remove(TElement item)
        {
            if (item is null)
                throw new ArgumentNullException(nameof(item));

            return this.m_elements.Remove(item);
        }

        public void RemoveAt(int index)
        {
            this.m_elements.RemoveAt(index);
        }

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

        public TElement this[string id]
        {
            get
            {
                int index = this.IndexOfID(id);
                if (index == -1)
                    throw new ArgumentOutOfRangeException(nameof(id), $"There is no element with id '{id}'");

                return this[index];
            }
            set
            {
                if (value is null)
                    throw new ArgumentNullException(nameof(value));

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
