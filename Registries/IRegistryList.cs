using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace AchievementsAPI.Registries
{
    public interface IRegistryList<TElement> : IList<TElement>
        where TElement : IRegisterable
    {
        TElement this[string id] { get; set; }
    }

    public static class IRegistryListExtensions
    {
        public static bool ContainsID<T>(this IRegistryList<T> list, string id)
            where T : IRegisterable
        {
            if (id is null)
                throw new ArgumentNullException(nameof(id));

            return list.IndexOfID(id) != -1;
        }
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

        public static T[] GetValues<T>(this IRegistryList<T> list, string id)
            where T : IRegisterable
        {
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
