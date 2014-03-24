namespace Topics.Radical.ComponentModel
{
	using System;

	/// <summary>
	/// A base class defining a generic strongly typed Timestamp type.
	/// </summary>
	/// <typeparam name="T">The type of the value holded by this Timestamp.</typeparam>
	public class Timestamp<T> : Timestamp
	{
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes" )]
		private readonly T value;
		
		/// <summary>
		/// Gets the underlying value holded by this instance.
		/// </summary>
		/// <value>The value holded by this instance.</value>
		public T Value
		{
			get { return this.value; }
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Timestamp&lt;T&gt;"/> class.
		/// </summary>
		/// <param name="value">The value.</param>
		public Timestamp( T value )
		{
			if( value == null )
			{
				throw new ArgumentNullException( "value", "Timestamp value cannot be null" );
			}

			this.value = value;
		}

		/// <summary>
		/// Performs an implicit conversion from the underlying type to <see cref="Topics.Radical.ComponentModel.Timestamp&lt;T&gt;"/>.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns>The result of the conversion.</returns>
		public static implicit operator Timestamp<T>( T value )
		{
			return new Timestamp<T>( value );
		}

		/// <summary>
		/// Performs an implicit conversion from <see cref="Topics.Radical.ComponentModel.Timestamp&lt;T&gt;"/> 
		/// to the underlying type.
		/// </summary>
		/// <param name="timestamp">The timestamp.</param>
		/// <returns>The result of the conversion.</returns>
		public static implicit operator T( Timestamp<T> timestamp )
		{
			if( timestamp == null )
			{
				return default( T );
			}

			return timestamp.value;
		}

		/// <summary>
		/// Determines whether the specified <see cref="T:Timestamp"/> is equal to the current <see cref="T:Timestamp"/>.
		/// </summary>
		/// <param name="obj">The <see cref="T:Timestamp"/> to compare with the current <see cref="T:Timestamp"/>.</param>
		/// <returns>
		/// true if the specified <see cref="T:Timestamp"/> is equal to the current <see cref="T:Timestamp"/>; otherwise, false.
		/// </returns>
		public override bool Equals( Timestamp obj )
		{
			Timestamp<T> timestamp = obj as Timestamp<T>;
			if( obj != null )
			{
				if( this.value is IComparable )
				{
					return ( ( IComparable )this.value ).CompareTo( timestamp.value ) == 0;
				}
				else
				{
					return Object.Equals( this.value, timestamp.value );
				}
			}
			else
			{
				return false;
			}
		}

		/// <summary>
		/// Serves as a hash function for a particular type.
		/// </summary>
		/// <returns> A hash code for the current System.Object.</returns>
		public override int GetHashCode()
		{
			unchecked
			{
				return ( ( this.value.GetHashCode() * 35 ) ^ 73 );
			}
		}
	}
}
