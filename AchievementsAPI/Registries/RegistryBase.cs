using AchievementsAPI.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AchievementsAPI.Registries
{
    /// <summary>
    /// A base registry implementation.
    /// <para>
    /// Doesn't handle the storage of actual data, so thats
    /// the job of implementing classes.
    /// </para>
    /// </summary>
    /// <typeparam name="T">The type of element this registry contains.</typeparam>
    public abstract class RegistryBase<T> : IRegistry<T>
        where T : IRegisterable
    {
        /// <summary>
        /// The Debug Name of this registry. Used for logging.
        /// </summary>
        protected virtual string DebugName => this.GetType().Name;

        /// <inheritdoc/>
        public abstract int Count { get; }
        /// <inheritdoc/>
        public virtual bool IsReadOnly => false;

        /// <inheritdoc/>
        public abstract IEnumerable<T> GetEntries();
        /// <inheritdoc/>
        public abstract IEnumerable<string> GetAllIDs();


        /// <summary>
        /// Returns whether or not an entry can be registered, outputting a reason if not.
        /// <para>
        /// Base implementation checks if the registry is read only,
        /// if the entry's ID is null,
        /// and if this registry already contains an entry with the entry's ID.
        /// </para>
        /// </summary>
        /// <param name="entry">The entry to check.</param>
        /// <param name="failReason">The reason why the entry can't be registered
        /// or <see langword="null"/></param>
        /// <returns><see langword="true"/> if the entry can be registered,
        /// otherwise <see langword="false"/></returns>
        protected virtual bool CanRegister(T entry, [NotNullWhen(false)] out string? failReason)
        {
            if (this.IsReadOnly)
            {
                failReason = "Registry is Read Only";
                return false;
            }

            string? id = entry.GetID();
            if (id is null)
            {
                failReason = "Entry ID is null.";
                return false;
            }

            if (this.ContainsEntry(id))
            {
                failReason = $"Entry with ID '{id}' already exists";
                return false;
            }

            failReason = null;
            return true;
        }

        /// <summary>
        /// Called when an entry is registered.
        /// </summary>
        /// <param name="entry">The entry registered.</param>
        protected virtual void OnRegistered(T entry)
        { }
        /// <summary>
        /// Called when an entry is unregistered.
        /// </summary>
        /// <param name="entry">The entry unregistered.</param>
        protected virtual void OnUnRegistered(T entry)
        { }
        /// <summary>
        /// Called when an entry fails to registered.
        /// </summary>
        /// <param name="entry">The entry that failed to register.</param>
        /// <param name="failReason">The reason why it failed to register.</param>
        protected virtual void OnRegisterFailed(T entry, string failReason)
        { }

        /// <inheritdoc/>
        public abstract bool TryGetEntry(string id, [NotNullWhen(true)] out T? entry);

        /// <summary>
        /// Return whether or not this registry has an entry with the specified ID.
        /// </summary>
        /// <param name="id">The ID of the entry to check for.</param>
        /// <returns><see langword="true"/> if it exists, otherwise
        /// <see langword="false"/></returns>
        protected abstract bool ContainsEntryWithID(string id);

        /// <summary>
        /// Adds an entry to the registry. No checks should be done here,
        /// as they are handled on methods that call this.
        /// </summary>
        /// <param name="entry">The entry to add.</param>
        protected abstract void AddEntry(T entry);

        /// <summary>
        /// Removes an entry to the registry. No checks should be done here,
        /// as they are handled on methods that call this.
        /// </summary>
        /// <param name="entry">The entry to remove.</param>
        protected virtual void RemoveEntry(T entry)
            => this.RemoveEntry(entry.GetID());

        /// <summary>
        /// Removes an entry to the registry. No checks should be done here,
        /// as they are handled on methods that call this.
        /// </summary>
        /// <param name="id">The ID of the entry to remove.</param>
        protected abstract void RemoveEntry(string id);

        /// <inheritdoc/>
        public void Register(T entry)
        {
            if (entry is null)
            {
                throw new ArgumentNullException(nameof(entry));
            }

            string? failReason;
            FailedToRegisterException? failEx = null;
            try
            {
                if (!this.CanRegister(entry, out failReason))
                {
                    failEx = new FailedToRegisterException(this, entry, failReason);
                }
            }
            catch (Exception ex)
            {
                failReason = "Failed verifying ability to register item";
                failEx = new FailedToRegisterException(this, entry, failReason, ex);
            }

            if (failReason != null)
            {
                this.InvokeOnRegisterFailed(entry, failReason);
                throw failEx!;
            }

            this.AddEntry(entry);
            this.InvokeOnRegistered(entry);
        }

        /// <inheritdoc/>
        public bool UnRegister(T entry)
        {
            if (entry is null)
            {
                throw new ArgumentNullException(nameof(entry));
            }

            return this.UnRegister(entry.GetID());
        }

        /// <inheritdoc/>
        public bool UnRegister(string id)
        {
            if (id is null)
            {
                throw new ArgumentNullException(nameof(id));
            }

            if (!this.ContainsEntry(id))
            {
                return false;
            }

            T entry = this[id];

            this.RemoveEntry(id);
            this.InvokeOnUnRegistered(entry);
            return true;
        }

        /// <inheritdoc/>
        public void UnRegisterAll()
        {
            string[] ids = this.GetAllIDs().ToArray();
            List<Exception> exceptions = new();

            foreach (string id in ids)
            {
                try
                {
                    this.UnRegister(id);
                }
                catch (Exception e)
                {
                    // manually remove it
                    if (this.ContainsEntryWithID(id))
                    {
                        this.RemoveEntry(id);
                    }

                    exceptions.Add(e);
                }
            }

            if (exceptions.Count == 0)
            {
                return;
            }

            if (exceptions.Count == 1)
            {
                throw new Exception("Failed to unregister an entry", exceptions[0]);
            }

            throw new AggregateException("Failed to unregister multiple entries", exceptions.ToArray());
        }

        /// <inheritdoc/>
        public bool ContainsEntry(string id)
            => id is null || this.ContainsEntryWithID(id);

        /// <inheritdoc/>
        public bool ContainsEntry(T entry)
            => (entry is not null) &&
            this.TryGetEntry(entry.GetID(), out T? item) &&
            item.Equals(entry);

        /// <inheritdoc/>
        public void CopyTo(T[] array, int index)
        {
            if (array is null)
            {
                throw new ArgumentNullException(nameof(array));
            }

            if (index + this.Count >= array.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            foreach (T entry in this.GetEntries())
            {
                array[index++] = entry;
            }
        }

        private void InvokeOnRegistered(T entry)
        {
            try
            {
                this.OnRegistered(entry);
            }
            catch (Exception softEx)
            {
                L.Debug($"[{this.DebugName}] Caught Exception whilst handling OnRegistered callback: " + softEx);
            }
        }

        private void InvokeOnRegisterFailed(T entry, string failReason)
        {
            try
            {
                this.OnRegisterFailed(entry, failReason);
            }
            catch (Exception softEx)
            {
                L.Debug($"[{this.DebugName}] Caught Exception whilst handling OnRegisterFail callback: " + softEx);
            }
        }

        private void InvokeOnUnRegistered(T entry)
        {
            try
            {
                this.OnUnRegistered(entry);
            }
            catch (Exception softEx)
            {
                L.Debug($"[{this.DebugName}] Caught Exception whilst handling OnUnRegistered callback: " + softEx);
            }
        }

        /// <inheritdoc/>
        public virtual T this[string id]
        {
            get
            {
                if (id is null)
                {
                    throw new ArgumentNullException(nameof(id));
                }

                if (this.TryGetEntry(id, out T? entry))
                {
                    return entry;
                }

                throw new KeyNotFoundException($"There is no entry with id '{id}'");
            }
        }

        /// <inheritdoc/>
        public IEnumerator<T> GetEnumerator()
            => this.GetEntries().GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator()
            => this.GetEnumerator();

        bool IRegistry.TryGetEntry(string id, [NotNullWhen(true)] out IRegisterable? entry)
        {
            if (this.TryGetEntry(id, out T? result))
            {
                entry = result;
                return true;
            }
            entry = default;
            return false;
        }

        void IRegistry.Register(IRegisterable entry)
        {
            if (entry is null)
            {
                throw new ArgumentNullException(nameof(entry));
            }

            if (entry is not T registerable)
            {
                throw new FailedToRegisterException(this, entry, $"Cannot register entries of type '{entry.GetType()}'");
            }

            this.Register(registerable);
        }
        bool IRegistry.ContainsEntry(IRegisterable entry)
            => (entry is not null) &&
            this.TryGetEntry(entry.GetID(), out T? item) &&
            item.Equals(entry);

        IEnumerable<IRegisterable> IRegistry.GetEntries()
        {
            return this.GetEntries()
                .Select((entry) => (IRegisterable)entry);
        }
        bool IRegistry.UnRegister(IRegisterable entry)
        {
            if (entry is null)
            {
                throw new ArgumentNullException(nameof(entry));
            }

            if (entry is not T unregisterable)
            {
                throw new InvalidCastException($"Cannot register entries of type '{entry.GetType()}'");
            }

            return this.UnRegister(unregisterable);
        }

        IRegisterable IRegistry.this[string id] => this[id];

        void ICollection<T>.Add(T item)
            => this.Register(item);

        bool ICollection<T>.Remove(T item)
            => this.UnRegister(item);

        void ICollection<T>.Clear()
            => this.UnRegisterAll();

        bool ICollection<T>.Contains(T item)
            => this.ContainsEntry(item);

        bool ICollection.IsSynchronized => false;
        object ICollection.SyncRoot => this;
        void ICollection.CopyTo(Array array, int index)
        {
            if (array is null)
            {
                throw new ArgumentNullException(nameof(array));
            }

            if (array.Rank != 1)
            {
                throw new ArgumentException($"Unsupported array rank '{array.Rank}' (only supports Rank 1)", nameof(array));
            }

            if (!array.GetType().GetElementType()!.IsAssignableFrom(typeof(T)))
            {
                throw new ArgumentException("Cannot add values to array, as types don't match", nameof(array));
            }

            if (index + this.Count >= array.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            foreach (T entry in this.GetEntries())
            {
                array.SetValue(entry, index++);
            }
        }
    }
}
