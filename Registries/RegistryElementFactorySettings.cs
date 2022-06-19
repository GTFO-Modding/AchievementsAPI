using System;

namespace AchievementsAPI.Registries
{
    /// <summary>
    /// Descriptor for creating a registry element.
    /// </summary>
    public class RegistryElementFactorySettings : IRegisterable
    {
        /// <summary>
        /// The type of the element.
        /// </summary>
        public Type Type { get; }
        /// <summary>
        /// The ID of the element.
        /// </summary>
        public string ID { get; }

        /// <summary>
        /// Initializes this descriptor.
        /// </summary>
        /// <param name="type">The type this descriptor creates.</param>
        /// <param name="id">The ID of the element.</param>
        /// <exception cref="ArgumentNullException"><paramref name="id"/>
        /// or <paramref name="type"/> is <see langword="null"/>.</exception>
        public RegistryElementFactorySettings(Type type, string id)
        {
            this.Type = type ?? throw new ArgumentNullException(nameof(type));
            this.ID = id ?? throw new ArgumentNullException(nameof(id));
        }

        /// <summary>
        /// Creates a new instance of the element this descriptor references.
        /// </summary>
        /// <returns>A new instance of the element this descriptor references.</returns>
        /// <exception cref="InvalidOperationException">If an instance failed to create.</exception>
        public IRegisterable CreateInstance() => (IRegisterable?)Activator.CreateInstance(this.Type) ?? throw new InvalidOperationException();

        string IRegisterable.GetID() => this.ID;
    }

    /// <summary>
    /// Descriptor for creating a registry element.
    /// </summary>
    /// <typeparam name="TElement"></typeparam>
    public class RegistryElementFactorySettings<TElement> : RegistryElementFactorySettings
        where TElement : IRegisterable
    {
        /// <summary>
        /// Initializes this descriptor.
        /// </summary>
        /// <param name="element">The element this descriptor is for.</param>
        public RegistryElementFactorySettings(TElement element) : base(element.GetType(), element.GetID())
        { }

        /// <summary>
        /// Creates a new instance of the element this descriptor references.
        /// </summary>
        /// <returns>A new instance of the element this descriptor references.</returns>
        /// <exception cref="InvalidOperationException">If an instance failed to create.</exception>
        new public TElement CreateInstance() => (TElement)base.CreateInstance();
    }
}
