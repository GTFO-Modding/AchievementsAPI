using System;

namespace AchievementsAPI.Registries
{
    public class RegistryElementFactorySettings : IRegisterable
    {
        public Type Type { get; }
        public string ID { get; }

        public RegistryElementFactorySettings(Type type, string id)
        {
            this.Type = type ?? throw new ArgumentNullException(nameof(type));
            this.ID = id ?? throw new ArgumentNullException(nameof(id));
        }

        public IRegisterable CreateInstance() => (IRegisterable?)Activator.CreateInstance(this.Type) ?? throw new InvalidOperationException();

        string IRegisterable.GetID() => this.ID;
    }

    public class RegistryElementFactorySettings<TElement> : RegistryElementFactorySettings
        where TElement : IRegisterable
    {
        public RegistryElementFactorySettings(TElement element) : base(element.GetType(), element.GetID())
        { }

        new public TElement CreateInstance() => (TElement)base.CreateInstance();
    }
}
