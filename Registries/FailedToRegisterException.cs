using System;
using System.Runtime.Serialization;

namespace AchievementsAPI.Registries
{
    public class FailedToRegisterException : Exception
    {
        public IRegistry? Registry { get; }
        public IRegisterable? Item { get; }

        protected FailedToRegisterException(SerializationInfo info, StreamingContext context) : base(info, context)
        { }

        public FailedToRegisterException() : base()
        { }

        public FailedToRegisterException(IRegistry registry) : base()
        {
            this.Registry = registry;
        }

        public FailedToRegisterException(IRegisterable item) : base()
        {
            this.Item = item;
        }

        public FailedToRegisterException(IRegistry registry, IRegisterable item) : base()
        {
            this.Registry = registry;
            this.Item = item;
        }

        public FailedToRegisterException(string message) : base(message)
        { }

        public FailedToRegisterException(IRegistry registry, string message) : base(message)
        {
            this.Registry = registry;
        }

        public FailedToRegisterException(IRegisterable item, string message) : base(message)
        {
            this.Item = item;
        }

        public FailedToRegisterException(IRegistry registry, IRegisterable item, string message) : base(message)
        {
            this.Registry = registry;
            this.Item = item;
        }

        public FailedToRegisterException(string message, Exception innerException) : base(message, innerException)
        { }

        public FailedToRegisterException(IRegisterable item, string message, Exception innerException) : base(message, innerException)
        {
            this.Item = item;
        }

        public FailedToRegisterException(IRegistry registry, string message, Exception innerException) : base(message, innerException)
        {
            this.Registry = registry;
        }

        public FailedToRegisterException(IRegistry registry, IRegisterable item, string message, Exception innerException) : base(message, innerException)
        {
            this.Registry = registry;
            this.Item = item;
        }
    }
}
