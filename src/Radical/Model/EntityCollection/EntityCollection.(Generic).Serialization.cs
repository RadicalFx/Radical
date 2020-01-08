using System.Globalization;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace Radical.Model
{
    partial class EntityCollection<T>
    {
        SerializationInfo serializationInfo = null;
        StreamingContext streamingContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityCollection&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="info">The info.</param>
        /// <param name="context">The context.</param>
        protected EntityCollection(SerializationInfo info, StreamingContext context)
            : this()
        {
            serializationInfo = info;
            streamingContext = context;

        }

        /// <summary>
        /// Populates a <see cref="T:System.Runtime.Serialization.SerializationInfo"/> with the data needed to serialize the target object.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"/> to populate with data.</param>
        /// <param name="context">The destination (see <see cref="T:System.Runtime.Serialization.StreamingContext"/>) for this serialization.</param>
        /// <exception cref="T:System.Security.SecurityException">
        /// The caller does not have the required permission.
        /// </exception>
        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            EnsureNotDisposed();
            OnGetObjectData(info, context);
        }

        /// <summary>
        /// Populates a <see cref="T:System.Runtime.Serialization.SerializationInfo"/> with the data needed to serialize the target object.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"/> to populate with data.</param>
        /// <param name="context">The destination (see <see cref="T:System.Runtime.Serialization.StreamingContext"/>) for this serialization.</param>
        /// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission. </exception>
        protected virtual void OnGetObjectData(SerializationInfo info, StreamingContext context)
        {
            EnsureNotDisposed();
            info.AddValue(SerializationKey, ToArray());
        }

        /// <summary>
        /// The serialization key to use during the serialization process.
        /// </summary>
        protected readonly static string SerializationKey = string.Format(CultureInfo.InvariantCulture, "{0}_Data_Array", typeof(T).Name);

        /// <summary>
        /// Runs when the entire object graph has been deserialized.
        /// </summary>
        /// <param name="sender">The object that initiated the callback. The functionality for this parameter is not currently implemented.</param>
        void IDeserializationCallback.OnDeserialization(object sender)
        {
            EnsureNotDisposed();
            T[] objs = (T[])serializationInfo.GetValue(SerializationKey, typeof(T[]));

            OnDeserialization(serializationInfo, streamingContext);

            foreach (T obj in objs)
            {
                Add(obj);
            }

            OnDeserializationCompleted(serializationInfo, streamingContext);

            serializationInfo = null;
        }

        /// <summary>
        /// Runs when the entire object graph has been deserialized.
        /// </summary>
        /// <param name="info">The serailization info.</param>
        /// <param name="context">The serailization context.</param>
        protected virtual void OnDeserialization(SerializationInfo info, StreamingContext context)
        {
            BeginInit();
        }

        /// <summary>
        /// Called when the deserialization process has been completed.
        /// </summary>
        /// <param name="info">The info.</param>
        /// <param name="context">The context.</param>
        protected virtual void OnDeserializationCompleted(SerializationInfo info, StreamingContext context)
        {
            EndInit();
        }
    }
}
