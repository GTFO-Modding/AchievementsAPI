using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace AchievementsAPI.Registries
{
    /// <summary>
    /// A list of registerable items.
    /// </summary>
    /// <typeparam name="TElement">The elements this list holds.</typeparam>
    public interface IRegistryList<TElement> : IList<TElement>
        where TElement : IRegisterable
    {
        /// <summary>
        /// Get or set values inside this list.
        /// </summary>
        /// <param name="id">The ID of the element.</param>
        /// <returns>The element at that ID</returns>
        /// <exception cref="KeyNotFoundException">Thrown if no such element
        /// with id <paramref name="id"/> exists</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="id"/>
        /// is <see langword="null"/></exception>
        TElement this[string id] { get; set; }
    }

    /// <summary>
    /// Extensions for registry lists.
    /// </summary>
    public static class IRegistryListExtensions
    {
        /// <summary>
        /// Returns whether or not the registry list contains
        /// an element with id <paramref name="id"/>.
        /// </summary>
        /// <typeparam name="T">The type of elements the list holds.</typeparam>
        /// <param name="list">The list instance.</param>
        /// <param name="id">The ID of the element to search for.</param>
        /// <returns><see langword="true"/> if an element with id <paramref name="id"/>
        /// was found, otherwise <see langword="false"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="id"/>
        /// is <see langword="null"/></exception>
        public static bool ContainsID<T>(this IRegistryList<T> list, string id)
            where T : IRegisterable
        {
            if (id is null)
                throw new ArgumentNullException(nameof(id));

            return list.IndexOfID(id) != -1;
        }
        /// <summary>
        /// Returns the first index of an element with id <paramref name="id"/>.
        /// </summary>
        /// <typeparam name="T">The type of elements the list holds.</typeparam>
        /// <param name="list">The list instance.</param>
        /// <param name="id">The ID of the element to find.</param>
        /// <returns>The index of the first found element with id <paramref name="id"/>,
        /// but if no element was found returns -1.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="id"/>
        /// is <see langword="null"/></exception>
        public static int IndexOfID<T>(this IRegistryList<T> list, string id)
            where T : IRegisterable
        {
            if (id is null)
                throw new ArgumentNullException(nameof(id));

            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].GetID() == id)
                {
                    return i;
                }
            }

            return -1;
        }
        /// <summary>
        /// Attempts to remove an element with the specified id.
        /// </summary>
        /// <typeparam name="T">The type of elements the list holds.</typeparam>
        /// <param name="list">The list instance.</param>
        /// <param name="id">The ID of the element to remove.</param>
        /// <returns><see langword="true"/> if an element with id <paramref name="id"/>
        /// was found and removed, otherwise <see langword="false"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="id"/>
        /// is <see langword="null"/></exception>
        public static bool RemoveID<T>(this IRegistryList<T> list, string id)
            where T : IRegisterable
        {
            if (id is null)
                throw new ArgumentNullException(nameof(id));

            int index = list.IndexOfID(id);
            if (index == -1)
                return false;

            list.RemoveAt(index);
            return true;
        }

        /// <summary>
        /// Tries to get an element in the list with the specified ID, returning
        /// whether or not it was successful.
        /// </summary>
        /// <typeparam name="T">The type of elements the list holds.</typeparam>
        /// <param name="list">The list instance.</param>
        /// <param name="id">The ID of the element to fetch.</param>
        /// <param name="value">The found value, or <see langword="null"/>.</param>
        /// <returns><see langword="true"/> if an element with id <paramref name="id"/>
        /// was found, otherwise <see langword="false"/>.</returns>
        public static bool TryGetValue<T>(this IRegistryList<T> list, string id, [NotNullWhen(true)] out T? value)
            where T : IRegisterable
        {
            int index = list.IndexOfID(id);
            if (index > -1)
            {
                value = list[index];
                return true;
            }
            else
            {
                value = default;
                return false;
            }
        }

        /// <summary>
        /// Returns a string array of all unique IDS this registry list has.
        /// </summary>
        /// <typeparam name="T">The type of elements the list holds.</typeparam>
        /// <param name="list">The list instance.</param>
        /// <returns>a string array of all unique IDS this registry list has.</returns>
        public static string[] GetIDS<T>(this IRegistryList<T> list)
            where T : IRegisterable
        {
            List<string> ids = new();

            foreach (T item in list)
            {
                string id = item.GetID();
                if (!ids.Contains(id))
                {
                    ids.Add(id);
                }
            }
            return ids.ToArray();
        }

        /// <summary>
        /// Returns all values associated with the specified id.
        /// </summary>
        /// <typeparam name="T">The type of elements the list holds.</typeparam>
        /// <param name="list">The list instance.</param>
        /// <param name="id">The ID.</param>
        /// <returns>all values associated with the specified id.</returns>
        public static T[] GetValues<T>(this IRegistryList<T> list, string id)
            where T : IRegisterable
        {
            if (id is null)
                return Array.Empty<T>();

            List<T> values = new();

            foreach (T item in list)
            {
                if (item.GetID() == id)
                {
                    values.Add(item);
                }
            }

            return values.ToArray();
        }
    }
}
