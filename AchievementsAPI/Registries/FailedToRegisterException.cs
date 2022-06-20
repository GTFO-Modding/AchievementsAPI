using System;
using System.Runtime.Serialization;

namespace AchievementsAPI.Registries
{
    /// <summary>
    /// An exception thrown when an element fails to register.
    /// </summary>
    public class FailedToRegisterException : Exception
    {
        /// <summary>
        /// The associated registry
        /// </summary>
        public IRegistry? Registry { get; }
        /// <summary>
        /// The item that failed to register.
        /// </summary>
        public IRegisterable? Item { get; }

        /// <summary>
        /// Initializes a new instance of this exception with serialized data.
        /// </summary>
        /// <param name="info">The seralization info.</param>
        /// <param name="context">The serialization context.</param>
        protected FailedToRegisterException(SerializationInfo info, StreamingContext context) : base(info, context)
        { }

        /// <summary>
        /// Initializes a new instance of this exception
        /// </summary>
        public FailedToRegisterException() : base()
        { }

        /// <summary>
        /// Initializes a new instance of this exception
        /// </summary>
        /// <param name="registry">The register that the item was attempted to be registered to.</param>
        public FailedToRegisterException(IRegistry registry) : base()
        {
            this.Registry = registry;
        }

        /// <summary>
        /// Initializes a new instance of this exception
        /// </summary>
        /// <param name="item">The item that was attempted to be registered</param>
        public FailedToRegisterException(IRegisterable item) : base()
        {
            this.Item = item;
        }

        /// <summary>
        /// Initializes a new instance of this exception
        /// </summary>
        /// <param name="registry">The register that the item was attempted to be registered to.</param>
        /// <param name="item">The item that was attempted to be registered</param>
        public FailedToRegisterException(IRegistry registry, IRegisterable item) : base()
        {
            this.Registry = registry;
            this.Item = item;
        }

        /// <summary>
        /// Initializes a new instance of this exception
        /// </summary>
        /// <param name="message">The message</param>
        public FailedToRegisterException(string message) : base(message)
        { }

        /// <summary>
        /// Initializes a new instance of this exception
        /// </summary>
        /// <param name="registry">The register that the item was attempted to be registered to.</param>
        /// <param name="message">The message</param>
        public FailedToRegisterException(IRegistry registry, string message) : base(message)
        {
            this.Registry = registry;
        }

        /// <summary>
        /// Initializes a new instance of this exception
        /// </summary>
        /// <param name="item">The item that was attempted to be registered</param>
        /// <param name="message">The message</param>
        public FailedToRegisterException(IRegisterable item, string message) : base(message)
        {
            this.Item = item;
        }

        /// <summary>
        /// Initializes a new instance of this exception
        /// </summary>
        /// <param name="registry">The register that the item was attempted to be registered to.</param>
        /// <param name="item">The item that was attempted to be registered</param>
        /// <param name="message">The message</param>
        public FailedToRegisterException(IRegistry registry, IRegisterable item, string message) : base(message)
        {
            this.Registry = registry;
            this.Item = item;
        }

        /// <summary>
        /// Initializes a new instance of this exception
        /// </summary>
        /// <param name="message">The message</param>
        /// <param name="innerException">The causing exception.</param>
        public FailedToRegisterException(string message, Exception innerException) : base(message, innerException)
        { }

        /// <summary>
        /// Initializes a new instance of this exception
        /// </summary>
        /// <param name="item">The item that was attempted to be registered</param>
        /// <param name="message">The message</param>
        /// <param name="innerException">The causing exception.</param>
        public FailedToRegisterException(IRegisterable item, string message, Exception innerException) : base(message, innerException)
        {
            this.Item = item;
        }

        /// <summary>
        /// Initializes a new instance of this exception
        /// </summary>
        /// <param name="registry">The register that the item was attempted to be registered to.</param>
        /// <param name="message">The message</param>
        /// <param name="innerException">The causing exception.</param>
        public FailedToRegisterException(IRegistry registry, string message, Exception innerException) : base(message, innerException)
        {
            this.Registry = registry;
        }

        /// <summary>
        /// Initializes a new instance of this exception
        /// </summary>
        /// <param name="registry">The register that the item was attempted to be registered to.</param>
        /// <param name="item">The item that was attempted to be registered</param>
        /// <param name="message">The message</param>
        /// <param name="innerException">The causing exception.</param>
        public FailedToRegisterException(IRegistry registry, IRegisterable item, string message, Exception innerException) : base(message, innerException)
        {
            this.Registry = registry;
            this.Item = item;
        }
    }
}
