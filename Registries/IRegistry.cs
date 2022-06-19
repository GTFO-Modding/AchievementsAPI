using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace AchievementsAPI.Registries
{
    /// <summary>
    /// Representation of a registry.
    /// </summary>
    public interface IRegistry : ICollection
    {
        /// <summary>
        /// Returns whether or not this registry contains an entry with the given id.
        /// </summary>
        /// <param name="id">The given ID</param>
        /// <returns><see langword="false"/> if <paramref name="id"/> is null or
        /// this registry doesn't contain an entry with id <paramref name="id"/>, otherwise
        /// <see langword="true"/></returns>
        bool ContainsEntry(string id);
        /// <summary>
        /// Returns whether or not this registry contains an entry with the given entry.
        /// </summary>
        /// <param name="entry">The given entry</param>
        /// <returns><see langword="false"/> if <paramref name="entry"/> is null or
        /// this registry doesn't contain an entry that doesn't match
        /// <paramref name="entry"/>, otherwise <see langword="true"/></returns>
        bool ContainsEntry(IRegisterable entry);

        /// <summary>
        /// Returns all entries in this registry.
        /// </summary>
        /// <returns>All entries in this registry.</returns>
        IEnumerable<IRegisterable> GetEntries();

        /// <summary>
        /// Returns all IDs of the entries in this registry.
        /// </summary>
        /// <returns>All IDs of the entries in this registry.</returns>
        IEnumerable<string> GetAllIDs();

        /// <summary>
        /// Gets an entry inside this registry. Could throw
        /// an exception.
        /// </summary>
        /// <param name="id">The ID of the entry to get.</param>
        /// <returns>The fetched entry.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="id"/>
        /// is <see langword="null"/></exception>
        /// <exception cref="KeyNotFoundException">Thrown if no entry with id
        /// <paramref name="id"/> exists in this entry.</exception>
        IRegisterable this[string id] { get; }

        /// <summary>
        /// Attempts to get an item in the registry with the given id <paramref name="id"/>.
        /// </summary>
        /// <param name="id">The ID of the entry to get.</param>
        /// <param name="entry">The entry gotten, or <see langword="default"/> if failed.</param>
        /// <returns><see langword="true"/> if getting the entry was successful, otherwise
        /// <see langword="false"/>.</returns>
        bool TryGetEntry(string id, [NotNullWhen(true)] out IRegisterable? entry);

        /// <summary>
        /// Registers the given entry in this registry.
        /// </summary>
        /// <param name="entry">The entry to register</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="entry"/>
        /// is <see langword="null"/></exception>
        /// <exception cref="FailedToRegisterException">Thrown if an error occurs
        /// whilst registering <paramref name="entry"/></exception>
        void Register(IRegisterable entry);

        /// <summary>
        /// Unregisters the entry with the id <paramref name="id"/> from the
        /// registry.
        /// </summary>
        /// <param name="id">The ID of the entry to unregister.</param>
        /// <returns><see langword="true"/> if an entry with ID <paramref name="id"/>
        /// was successfully unregistered and removed from this registry, otherwise
        /// returns <see langword="false"/> if there was no entry with id <paramref name="id"/>
        /// in the registry to begin with.</returns>
        bool UnRegister(string id);

        /// <summary>
        /// Attempts to unregister <paramref name="entry"/> from this registry.
        /// </summary>
        /// <param name="entry">The entry to unregister.</param>
        /// <returns><see langword="true"/> if <paramref name="entry"/> was successfully
        /// unregistered and removed from this registry, otherwise returns
        /// <see langword="false"/> if <paramref name="entry"/> wasn't in the registry
        /// to begin with.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="entry"/>
        /// is <see langword="null"/></exception>
        bool UnRegister(IRegisterable entry);

        /// <summary>
        /// Unregisters all entries in this registry. Pretty much clears this registry.
        /// <para>
        /// This method will throw exceptions after all entries are unregistered, so you can
        /// ensure that this registry will be empty. If one exception occurred, it will rethrow it,
        /// otherwise it will throw an <see cref="AggregateException"/>.
        /// </para>
        /// </summary>
        /// <exception cref="Exception">Thrown for an individual unregister error</exception>
        /// <exception cref="AggregateException">Thrown for multiple unregister errors</exception>
        void UnRegisterAll();
    }

    /// <summary>
    /// Representation of a registry of specific elements.
    /// </summary>
    /// <typeparam name="T">The element this registry contains.</typeparam>
    public interface IRegistry<T> : IRegistry, ICollection<T>
        where T : IRegisterable
    {
        /// <summary>
        /// Returns whether or not this registry contains an entry with the given entry.
        /// </summary>
        /// <param name="entry">The given entry</param>
        /// <returns><see langword="false"/> if <paramref name="entry"/> is null or
        /// this registry doesn't contain an entry that doesn't match
        /// <paramref name="entry"/>, otherwise <see langword="true"/></returns>
        bool ContainsEntry(T entry);

        /// <summary>
        /// Attempts to unregister <paramref name="entry"/> from this registry.
        /// </summary>
        /// <param name="entry">The entry to unregister.</param>
        /// <returns><see langword="true"/> if <paramref name="entry"/> was successfully
        /// unregistered and removed from this registry, otherwise returns
        /// <see langword="false"/> if <paramref name="entry"/> wasn't in the registry
        /// to begin with.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="entry"/>
        /// is <see langword="null"/></exception>
        bool UnRegister(T entry);

        /// <summary>
        /// Returns all entries in this registry.
        /// </summary>
        /// <returns>All entries in this registry.</returns>
        new IEnumerable<T> GetEntries();

        /// <summary>
        /// Registers the given entry in this registry.
        /// </summary>
        /// <param name="entry">The entry to register</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="entry"/>
        /// is <see langword="null"/></exception>
        /// <exception cref="FailedToRegisterException">Thrown if an error occurs
        /// whilst registering <paramref name="entry"/></exception>
        void Register(T entry);

        /// <summary>
        /// Attempts to get an item in the registry with the given id <paramref name="id"/>.
        /// </summary>
        /// <param name="id">The ID of the entry to get.</param>
        /// <param name="entry">The entry gotten, or <see langword="default"/> if failed.</param>
        /// <returns><see langword="true"/> if getting the entry was successful, otherwise
        /// <see langword="false"/>.</returns>
        bool TryGetEntry(string id, [NotNullWhen(true)] out T? entry);

        /// <summary>
        /// Gets an item in this registry with the specified ID.
        /// </summary>
        /// <param name="id">The ID of the item to get.</param>
        /// <returns>The found item.</returns>
        /// <exception cref="KeyNotFoundException">Thrown if no such entry
        /// with id <paramref name="id"/> exists</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="id"/>
        /// is <see langword="null"/></exception>
        new T this[string id] { get; }
    }
}
