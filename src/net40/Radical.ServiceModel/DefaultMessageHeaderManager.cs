using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using Topics.Radical.Validation;
using Topics.Radical.Conversions;
using System.ServiceModel.Channels;
using System.ComponentModel;
using System.Reflection;
using Topics.Radical.Reflection;

namespace Topics.Radical.ServiceModel
{
	/// <summary>
	/// Implements the <see cref="IMessageHeaderManager"/> providing
	/// a set of default functionalities.
	/// </summary>
	public class DefaultMessageHeaderManager : IMessageHeaderManager
	{
		OperationContextScope scope;

		#region IDisposable Members

		/// <summary>
		/// Releases unmanaged resources and performs other cleanup operations before the
		/// <see cref="DefaultMessageHeaderManager"/> is reclaimed by garbage collection.
		/// </summary>
		~DefaultMessageHeaderManager()
		{
			this.Dispose( false );
		}

		private Boolean isDisposed;

		/// <summary>
		/// Releases unmanaged and - optionally - managed resources
		/// </summary>
		/// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
		protected virtual void Dispose( Boolean disposing )
		{
			if( disposing )
			{
				if( this.scope != null )
				{
					this.scope.Dispose();
				}
			}

			this.scope = null;
			this.isDisposed = true;
		}

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		public void Dispose()
		{
			this.Dispose( true );
			GC.SuppressFinalize( this );
		}

		/// <summary>
		/// Verifies that this instance is not disposed, throwing an
		/// <see cref="ObjectDisposedException"/> if this instance has
		/// been already disposed.
		/// </summary>
		protected virtual void EnsureNotDisposed()
		{
			if( this.isDisposed )
			{
				throw new ObjectDisposedException( this.GetType().FullName );
			}
		}

		#endregion

		/// <summary>
		/// Initializes a new instance of the <see cref="DefaultMessageHeaderManager"/> class.
		/// </summary>
		/// <param name="channel">The channel.</param>
		public DefaultMessageHeaderManager( IContextChannel channel )
		{
			Ensure.That( channel ).Named( () => channel ).IsNotNull();

			this.scope = new OperationContextScope( channel );
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="DefaultMessageHeaderManager"/> class.
		/// </summary>
		public DefaultMessageHeaderManager()
		{

		}

		Boolean Exists( MessageHeaders headers, String headerName, String headerNamespace )
		{
			var index = headers.FindHeader( headerName, headerNamespace );

			return index != -1;
		}

		/// <summary>
		/// Sets the header in current operation scope.
		/// </summary>
		/// <typeparam name="T">The type of the header.</typeparam>
		/// <param name="header">The header to add to the operation context.</param>
		public void SetHeader<T>( T header )
		{
			var mh = new MessageHeader<T>( header );
			OperationContext.Current.OutgoingMessageHeaders.Add
			(
				mh.GetUntypedHeader
				(
					this.GetHeaderUniqueName<T>(),
					this.GetHeaderNamespace<T>()
				)
			);
		}

		/// <summary>
		/// Gets the unique name of the header to use during the 
		/// enlinstment of the header into the message.
		/// </summary>
		/// <typeparam name="T">The type of the header.</typeparam>
		/// <returns>The name.</returns>
		protected virtual String GetHeaderUniqueName<T>()
		{
			var dcName = typeof( T ).GetAttribute<DataContractAttribute>().Name;
			if( String.IsNullOrEmpty( dcName ) )
			{
				return typeof( T ).Name;
			}

			return dcName;
		}

		/// <summary>
		/// Gets the header namespace to use during the 
		/// enlinstment of the header into the message.
		/// </summary>
		/// <typeparam name="T">The type of the header.</typeparam>
		/// <returns>The namespace.</returns>
		protected virtual String GetHeaderNamespace<T>()
		{
			var dcNamespace = typeof( T ).GetAttribute<DataContractAttribute>().Namespace;
			if( String.IsNullOrEmpty( dcNamespace ) )
			{
				return typeof( T ).Namespace;
			}

			return dcNamespace;
		}

		/// <summary>
		/// Gets the incoming header of the given type.
		/// </summary>
		/// <typeparam name="T">The type of the header.</typeparam>
		/// <returns>The header if any; otherwise null.</returns>
		public T GetIncomingHeader<T>()
		{
			return this.GetIncomingHeader<T>( HeaderSearchBehavior.Optional );
		}

		/// <summary>
		/// Gets the outgoing header of the given type.
		/// </summary>
		/// <typeparam name="T">The type of the header.</typeparam>
		/// <returns>The header if any; otherwise null.</returns>
		public T GetOutgoingHeader<T>()
		{
			return this.GetOutgoingHeader<T>( HeaderSearchBehavior.Optional );
		}

		/// <summary>
		/// Gets the header of the given type first searching in the
		/// incoming headers and then, if no header can be found, in
		/// the outgoing headers.
		/// </summary>
		/// <typeparam name="T">The type of the header.</typeparam>
		/// <returns>The header if any; otherwise null.</returns>
		public T GetHeader<T>()
		{
			return this.GetHeader<T>( HeaderSearchBehavior.Optional );
		}

		/// <summary>
		/// Gets the header of the given type first searching in the
		/// incoming headers and then, if no header can be found, in
		/// the outgoing headers.
		/// </summary>
		/// <typeparam name="T">The type of the header.</typeparam>
		/// <param name="searchBehavior">if set to <c>Mandatory</c> then the
		/// header is considered a mandatory header and if no header
		/// of the given type can be found an exception will be raised.</param>
		/// <returns>The header if any; otherwise null.</returns>
		public T GetHeader<T>( HeaderSearchBehavior searchBehavior )
		{
			T header;
			if( this.TryGetHeader<T>( out header ) )
			{
				return header;
			}

			if( searchBehavior == HeaderSearchBehavior.Mandatory )
			{
				throw new InvalidMessageContractException
				(
					String.Format( "No header of type '{0}' can be found in the WCF message header.", typeof( T ) )
				);
			}

			return default( T );
		}

		/// <summary>
		/// Gets the outgoing header of the given type.
		/// </summary>
		/// <typeparam name="T">The type of the header.</typeparam>
		/// <param name="searchBehavior">if set to <c>Mandatory</c> then the
		/// header is considered a mandatory header and if no header
		/// of the given type can be found an exception will be raised.</param>
		/// <returns>The header if any; otherwise null.</returns>
		public T GetOutgoingHeader<T>( HeaderSearchBehavior searchBehavior )
		{
			T header;
			if( this.TryGetOutgoingHeader<T>( out header ) )
			{
				return header;
			}

			if( searchBehavior == HeaderSearchBehavior.Mandatory )
			{
				throw new InvalidMessageContractException
				(
					String.Format( "No header of type '{0}' can be found in the outgoing header.", typeof( T ) )
				);
			}

			return default( T );
		}

		/// <summary>
		/// Gets the incoming header of the given type.
		/// </summary>
		/// <typeparam name="T">The type of the header.</typeparam>
		/// <param name="searchBehavior">if set to <c>Mandatory</c> then the
		/// header is considered a mandatory header and if no header
		/// of the given type can be found an exception will be raised.</param>
		/// <returns>The header if any; otherwise null.</returns>
		public T GetIncomingHeader<T>( HeaderSearchBehavior searchBehavior )
		{
			T header;
			if( this.TryGetIncomingHeader<T>( out header ) )
			{
				return header;
			}

			if( searchBehavior == HeaderSearchBehavior.Mandatory )
			{
				throw new InvalidMessageContractException
				(
					String.Format( "No header of type '{0}' can be found in the incoming header.", typeof( T ) )
				);
			}

			return default( T );
		}


		/// <summary>
		/// Gets the header of the given type first searching in the
		/// incoming headers and then, if no header can be found, in
		/// the outgoing headers.
		/// </summary>
		/// <typeparam name="T">The type of the header.</typeparam>
		/// <param name="header">The header if any; otherwise null.</param>
		/// <returns>
		/// 	<c>True</c> if the requested header has been found; otherwise false.
		/// </returns>
		public bool TryGetHeader<T>( out T header )
		{
			if( this.TryGetIncomingHeader<T>( out header ) )
			{
				return true;
			}

			if( this.TryGetOutgoingHeader<T>( out header ) )
			{
				return true;
			}

			header = default( T );
			return false;
		}

		/// <summary>
		/// Gets the outgoing header of the given type.
		/// </summary>
		/// <typeparam name="T">The type of the header.</typeparam>
		/// <param name="header">The header if any; otherwise null.</param>
		/// <returns>
		/// 	<c>True</c> if the requested header has been found; otherwise false.
		/// </returns>
		public bool TryGetOutgoingHeader<T>( out T header )
		{
			if( OperationContext.Current.OutgoingMessageHeaders != null )
			{
				var exists = this.Exists
				(
					OperationContext.Current.OutgoingMessageHeaders,
					this.GetHeaderUniqueName<T>(),
					this.GetHeaderNamespace<T>()
				);

				if( exists )
				{
					header = OperationContext.Current.OutgoingMessageHeaders.GetHeader<T>
					(
						this.GetHeaderUniqueName<T>(),
						this.GetHeaderNamespace<T>()
					);

					header.As<INeedValidationHeader>( i => i.Validate() );

					return true;
				}
			}

			header = default( T );
			return false;
		}

		/// <summary>
		/// Gets the incoming header of the given type.
		/// </summary>
		/// <typeparam name="T">The type of the header.</typeparam>
		/// <param name="header">The header if any; otherwise null.</param>
		/// <returns>
		/// 	<c>True</c> if the requested header has been found; otherwise false.
		/// </returns>
		public bool TryGetIncomingHeader<T>( out T header )
		{
			if( OperationContext.Current.IncomingMessageHeaders != null )
			{
				var exists = this.Exists
				(
					OperationContext.Current.IncomingMessageHeaders,
					this.GetHeaderUniqueName<T>(),
					this.GetHeaderNamespace<T>()
				);

				if( exists )
				{
					header = OperationContext.Current.IncomingMessageHeaders.GetHeader<T>
					(
						this.GetHeaderUniqueName<T>(),
						this.GetHeaderNamespace<T>()
					);

					header.As<INeedValidationHeader>( i => i.Validate() );

					return true;
				}
			}

			header = default( T );
			return false;
		}
	}
}
