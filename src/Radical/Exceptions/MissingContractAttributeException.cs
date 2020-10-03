namespace Radical
{
    using Radical.Reflection;
    using System;
    using System.Globalization;
    using System.Runtime.Serialization;
    using System.Security.Permissions;

    /// <summary>
    /// Exception raised to notify that the <see cref="Radical.ComponentModel.ContractAttribute"/>
    /// cannot be found on the searched entity.
    /// </summary>
    [Serializable]
    public class MissingContractAttributeException : RadicalException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MissingContractAttributeException"/> class.
        /// </summary>
        /// <param name="info">The object that holds the serialized object data.</param>
        /// <param name="context">The contextual information about the source or destination.</param>
        protected MissingContractAttributeException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            Type targetType = Type.GetType(info.GetString("targetType"));
            TargetType = targetType;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MissingContractAttributeException"/> class.
        /// </summary>
        /// <param name="targetType">The target type.</param>
        public MissingContractAttributeException(Type targetType)
            : this(targetType ?? throw new ArgumentNullException(nameof(targetType)), string.Format(CultureInfo.CurrentCulture, "ContractAttribute missing on type: {0}.", targetType.FullName))
        {
            TargetType = targetType;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MissingContractAttributeException"/> class.
        /// </summary>
        /// <param name="targetType">The target type.</param>
        /// <param name="message">The message.</param>
        public MissingContractAttributeException(Type targetType, string message)
            : base(message)
        {
            TargetType = targetType;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MissingContractAttributeException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        public MissingContractAttributeException(string message, Exception innerException)
            : base(message, innerException)
        {

        }

        /// <summary>
        /// Gets the type on witch the contract attribute is missing.
        /// </summary>
        /// <value>The type on witch the contract attribute is missing.</value>
        public Type TargetType { get; }

        /// <summary>
        /// When overridden in a derived class, sets the <see>
        ///     <cref>T:System.Runtime.Serialization.SerializationInfo</cref>
        /// </see>
        /// with information about the exception.
        /// </summary>
        /// <param name="info">The <see>
        ///         <cref>T:System.Runtime.Serialization.SerializationInfo</cref>
        ///     </see>
        ///     that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see>
        ///         <cref>T:System.Runtime.Serialization.StreamingContext</cref>
        ///     </see>
        ///     that contains contextual information about the source or destination.</param>
        /// <exception>
        /// The
        /// <cref>T:System.ArgumentNullException</cref>
        /// <paramref name="info"/> parameter is a null reference (Nothing in Visual Basic).
        /// </exception>
        /// <PermissionSet>
        ///     <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Read="*AllFiles*" PathDiscovery="*AllFiles*"/>
        ///     <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="SerializationFormatter"/>
        /// </PermissionSet>
        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);

            info.AddValue("targetType", TargetType.ToShortString(), typeof(string));
        }
    }
}
