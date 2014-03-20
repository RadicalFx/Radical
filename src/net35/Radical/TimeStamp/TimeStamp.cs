namespace Topics.Radical.ComponentModel
{
	using System;

	/// <summary>
	/// A base class defining a Timestamp type.
	/// </summary>
	public abstract class Timestamp
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="Timestamp"/> class.
		/// </summary>
		protected Timestamp()
		{

		}

		/// <summary>
		/// Implements the operator ==.
		/// </summary>
		/// <param name="leftValue">The left side timestamp.</param>
		/// <param name="rightValue">The right side timestamp.</param>
		/// <returns>The result of the operator.</returns>
		public static Boolean operator ==( Timestamp leftValue, Timestamp rightValue )
		{
			Boolean returnValue = true;

			if( Object.Equals( leftValue, null ) && Object.Equals( rightValue, null ) )
			{
				/*
				 * Sono entrambi null --> sono uguali
				 */
				returnValue = true;
			}
			//else if( ( Object.Equals( leftvalue, null ) && !Object.Equals( v2, null ) ) || ( !Object.Equals( leftvalue, null ) && Object.Equals( v2, null ) ) )
			else if( ( Object.Equals( leftValue, null ) || Object.Equals( rightValue, null ) ) )
			{
				/*
				 * leftvalue == null
				 * v2 != null --> non sono uguali
				 * 
				 * leftvalue != null
				 * v2 == null --> non sono uguali
				 */
				returnValue = false;
			}
			else
			{
				returnValue = leftValue.Equals( rightValue );
			}

			return returnValue;
		}

		/// <summary>
		/// Determines whether the specified <see cref="T:Timestamp"/> is equal to the current <see cref="T:Timestamp"/>.
		/// </summary>
		/// <param name="obj">The <see cref="T:Timestamp"/> to compare with the current <see cref="T:Timestamp"/>.</param>
		/// <returns>
		/// true if the specified <see cref="T:Timestamp"/> is equal to the current <see cref="T:Timestamp"/>; otherwise, false.
		/// </returns>
		public abstract Boolean Equals( Timestamp obj );

		/// <summary>
		/// Implements the operator !=.
		/// </summary>
		/// <param name="leftValue">The left side timestamp.</param>
		/// <param name="rightValue">The right side timestamp.</param>
		/// <returns>The result of the operator.</returns>
		public static Boolean operator !=( Timestamp leftValue, Timestamp rightValue )
		{
			return !( leftValue == rightValue );
		}

		/// <summary>
		/// Determines whether the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>.
		/// </summary>
		/// <param name="obj">The <see cref="T:System.Object"/> to compare with the current <see cref="T:System.Object"/>.</param>
		/// <returns>
		/// true if the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>; otherwise, false.
		/// </returns>
		public override sealed Boolean Equals( Object obj )
		{
			Timestamp target = obj as Timestamp;
			if( target != null )
			{
				return this.Equals( target );
			}

			return false;
		}

		/// <summary>
		/// Serves as a hash function for a particular type.
		/// </summary>
		/// <returns>
		/// A hash code for the current <see cref="T:System.Object"/>.
		/// </returns>
		public override abstract int GetHashCode();
	}
}
