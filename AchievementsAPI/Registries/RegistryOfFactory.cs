using AchievementsAPI.Conditions.Registries;
using AchievementsAPI.Triggers.Registries;
using AchievementsAPI.Utilities;
using System;
using System.Diagnostics.CodeAnalysis;

namespace AchievementsAPI.Registries
{
    /// <summary>
    /// Implementation of a Registry of Descriptors that can be used to
    /// instantiate a new element. Primarily used by <see cref="TriggerRegistry"/>
    /// and <see cref="ConditionRegistry"/>.
    /// </summary>
    /// <typeparam name="TElement">The element each descriptor creates</typeparam>
    /// <typeparam name="TElementFactoryInfo">The type of the descriptor</typeparam>
    public abstract class RegistryOfFactory<TElement, TElementFactoryInfo> : Registry<TElementFactoryInfo>
        where TElementFactoryInfo : RegistryElementFactorySettings<TElement>
        where TElement : IRegisterable
    {
        /// <summary>
        /// Create a new Descriptor based on the element provided.
        /// </summary>
        /// <param name="element">The element</param>
        /// <returns>A descriptor of that element.</returns>
        protected abstract TElementFactoryInfo CreateFactoryInfo(TElement element);

        /// <summary>
        /// Register an element in this registry factory.
        /// </summary>
        /// <typeparam name="T">The type of the element.</typeparam>
        public void Register<T>()
            where T : TElement, new()
        {
            this.Register(new T());
        }
        /// <summary>
        /// Register an element in this registry factory.
        /// </summary>
        /// <exception cref="ArgumentNullException">When <paramref name="element"/>
        /// is <see langword="null"/></exception>
        public void Register(TElement element)
        {
            if (element is null)
            {
                throw new ArgumentNullException(nameof(element));
            }

            this.Register(this.CreateFactoryInfo(element));
        }

        /// <summary>
        /// Unregister an element in this registry factory
        /// </summary>
        /// <exception cref="ArgumentNullException">When <paramref name="element"/>
        /// is <see langword="null"/></exception>
        public bool UnRegister(TElement element)
        {
            if (element is null)
            {
                throw new ArgumentNullException(nameof(element));
            }

            return this.UnRegister(element.GetID());
        }

        /// <summary>
        /// Create a new element using the descriptor with id
        /// <paramref name="id"/>.
        /// </summary>
        /// <param name="id">The ID of the descriptor</param>
        /// <returns>The created element.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="id"/>
        /// is <see langword="null"/></exception>
        /// <exception cref="System.Collections.Generic.KeyNotFoundException">
        /// No such element with id <paramref name="id"/> exists.</exception>
        public TElement CreateElement(string id)
            => this[id].CreateInstance();

        /// <summary>
        /// Attempts to create an element, returning false if failed.
        /// </summary>
        /// <param name="id">The ID of the descriptor</param>
        /// <param name="element">The created element, or <see langword="default"/>
        /// if it failed to create.</param>
        /// <returns><see langword="true"/> if the element was created successfully,
        /// otherwise <see langword="false"/>.</returns>
        public bool TryCreateElement(string id, [NotNullWhen(true)] out TElement? element)
        {
            if (this.TryGetEntry(id, out TElementFactoryInfo? settings))
            {
                try
                {
                    element = settings.CreateInstance();
                    return true;
                }
                catch (Exception ex)
                {
                    L.Warn($"Failed creating element for factory with id '{id}': " + ex);
                    goto fail;
                }
            }

        fail:
            element = default;
            return false;
        }
    }

    /// <summary>
    /// Implementation of a Registry of Descriptors that can be used to
    /// instantiate a new element.
    /// <para>
    /// Uses the default descriptor <see cref="RegistryElementFactorySettings{TElement}"/>.
    /// </para>
    /// </summary>
    /// <typeparam name="TElement">The element each descriptor creates</typeparam>
    public class RegistryOfFactory<TElement> : RegistryOfFactory<TElement, RegistryElementFactorySettings<TElement>>
        where TElement : IRegisterable
    {
        /// <inheritdoc/>
        protected sealed override RegistryElementFactorySettings<TElement> CreateFactoryInfo(TElement element)
        {
            return new(element);
        }
    }
}
