using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace AchievementsAPI.Registries
{
    public class Registry<T> : RegistryBase<T>
        where T : IRegisterable
    {
        private readonly Dictionary<string, T> m_entries;

        public Registry()
        {
            this.m_entries = new();
        }

        public override int Count => this.m_entries.Count;
        public override IEnumerable<T> GetEntries()
        {
            return this.m_entries.Values;
        }
        public override IEnumerable<string> GetAllIDs()
        {
            return this.m_entries.Keys;
        }
        public override bool TryGetEntry(string id, [NotNullWhen(true)] out T? entry)
            => this.m_entries.TryGetValue(id, out entry);

        protected sealed override void AddEntry(T entry)
            => this.m_entries.Add(entry.GetID(), entry);
        protected sealed override void RemoveEntry(string id)
            => this.m_entries.Remove(id);
        protected override bool ContainsEntryWithID(string id)
            => this.m_entries.ContainsKey(id);
    }
}
