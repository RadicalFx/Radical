using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Runtime.Serialization;
using System.Security.Permissions;
using Topics.Radical.ComponentModel;
using Topics.Radical.Linq;
using Topics.Radical.Validation;
using System.Linq.Expressions;

namespace Topics.Radical.Model
{
    partial class EntityCollection<T>
    {
#if !SILVERLIGHT

        SerializationInfo serializationInfo = null;
        StreamingContext streamingContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityCollection&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="info">The info.</param>
        /// <param name="context">The context.</param>
        protected EntityCollection( SerializationInfo info, StreamingContext context )
            : this()
        {
            this.serializationInfo = info;
            this.streamingContext = context;

        }

        /// <summary>
        /// Populates a <see cref="T:System.Runtime.Serialization.SerializationInfo"/> with the data needed to serialize the target object.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"/> to populate with data.</param>
        /// <param name="context">The destination (see <see cref="T:System.Runtime.Serialization.StreamingContext"/>) for this serialization.</param>
        /// <exception cref="T:System.Security.SecurityException">
        /// The caller does not have the required permission.
        /// </exception>
        [SecurityPermission( SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter )]
        void ISerializable.GetObjectData( SerializationInfo info, StreamingContext context )
        {
            this.EnsureNotDisposed();
            this.OnGetObjectData( info, context );
        }

        /// <summary>
        /// Populates a <see cref="T:System.Runtime.Serialization.SerializationInfo"/> with the data needed to serialize the target object.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"/> to populate with data.</param>
        /// <param name="context">The destination (see <see cref="T:System.Runtime.Serialization.StreamingContext"/>) for this serialization.</param>
        /// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission. </exception>
        protected virtual void OnGetObjectData( SerializationInfo info, StreamingContext context )
        {
            this.EnsureNotDisposed();
            info.AddValue( SerializationKey, this.ToArray() );
        }

        /// <summary>
        /// The serialization key to use during the serialization process.
        /// </summary>
        protected readonly static String SerializationKey = String.Format( CultureInfo.InvariantCulture, "{0}_Data_Array", typeof( T ).Name );

        /// <summary>
        /// Runs when the entire object graph has been deserialized.
        /// </summary>
        /// <param name="sender">The object that initiated the callback. The functionality for this parameter is not currently implemented.</param>
        void IDeserializationCallback.OnDeserialization( object sender )
        {
            this.EnsureNotDisposed();
            T[] objs = ( T[] )serializationInfo.GetValue( SerializationKey, typeof( T[] ) );

            this.OnDeserialization( this.serializationInfo, this.streamingContext );

            foreach( T obj in objs )
            {
                this.Add( obj );
            }

            this.OnDeserializationCompleted( this.serializationInfo, this.streamingContext );

            this.serializationInfo = null;
        }

        /// <summary>
        /// Runs when the entire object graph has been deserialized.
        /// </summary>
        /// <param name="info">The serailization info.</param>
        /// <param name="context">The serailization context.</param>
        protected virtual void OnDeserialization( SerializationInfo info, StreamingContext context )
        {
            this.BeginInit();
        }

        /// <summary>
        /// Called when the deserialization process has been completed.
        /// </summary>
        /// <param name="info">The info.</param>
        /// <param name="context">The context.</param>
        protected virtual void OnDeserializationCompleted( SerializationInfo info, StreamingContext context )
        {
            this.EndInit();
        }

#endif
    }
}
