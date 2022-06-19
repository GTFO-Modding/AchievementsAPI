using AchievementsAPI.Utilities;
using System;
using System.Diagnostics.CodeAnalysis;

namespace AchievementsAPI.Registries
{
    public class RegistryOfFactory<TElement> : Registry<RegistryElementFactorySettings<TElement>>
        where TElement : IRegisterable
    {
        public void Register<T>()
            where T : TElement, new()
        {
            this.Register(new T());
        }
        public void Register(TElement element)
        {
            if (element is null)
                throw new ArgumentNullException(nameof(element));

            this.Register(new RegistryElementFactorySettings<TElement>(element));
        }

        public bool UnRegister(TElement element)
        {
            if (element is null)
                throw new ArgumentNullException(nameof(element));

            return this.UnRegister(element.GetID());
        }

        public TElement CreateElement(string id)
            => this[id].CreateInstance();

        public bool TryCreateElement(string id, [NotNullWhen(true)] out TElement? element)
        {
            if (this.TryGetEntry(id, out var settings))
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
}
