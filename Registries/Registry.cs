using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace AchievementsAPI.Registries
{
    /// <summary>
    /// A registry implementation using a dictionary.
    /// </summary>
    /// <typeparam name="T">The type of element this registry stores</typeparam>
    public class Registry<T> : RegistryBase<T>
        where T : IRegisterable
    {
        private readonly Dictionary<string, T> m_entries;

        /// <summary>
        /// Initializes this registru.
        /// </summary>
        public Registry()
        {
            this.m_entries = new();
        }

        /// <inheritdoc/>
        public override int Count => this.m_entries.Count;
        /// <inheritdoc/>
        public override IEnumerable<T> GetEntries()
        {
            return this.m_entries.Values;
        }
        /// <inheritdoc/>
        public override IEnumerable<string> GetAllIDs()
        {
            return this.m_entries.Keys;
        }
        /// <inheritdoc/>
        public override bool TryGetEntry(string id, [NotNullWhen(true)] out T? entry)
            => this.m_entries.TryGetValue(id, out entry);

        /// <inheritdoc/>
        protected sealed override void AddEntry(T entry)
            => this.m_entries.Add(entry.GetID(), entry);
        /// <inheritdoc/>
        protected sealed override void RemoveEntry(string id)
            => this.m_entries.Remove(id);
        /// <inheritdoc/>
        protected override bool ContainsEntryWithID(string id)
            => this.m_entries.ContainsKey(id);
    }
}
