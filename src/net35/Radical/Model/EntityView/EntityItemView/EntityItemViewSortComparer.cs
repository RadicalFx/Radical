namespace Topics.Radical.Model
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel;
	using Topics.Radical.ComponentModel;

	public class EntityItemViewSortComparer<T> : IComparer<IEntityItemView<T>>
		//where T : class
	{
		/// <summary>
		/// Gets the sort descriptions.
		/// </summary>
		/// <item>The sort descriptions.</item>
		protected ListSortDescriptionCollection SortDescriptions
		{
			get;
			private set;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="EntityItemViewSortComparer&lt;T&gt;"/> class.
		/// </summary>
		/// <param name="sortDescriptions">The sort descriptions.</param>
		public EntityItemViewSortComparer( ListSortDescriptionCollection sortDescriptions )
		{
			this.SortDescriptions = sortDescriptions;
		}

		static Int32 Compare( Object a, Object b, ListSortDirection direction )
		{
			Int32 retVal = 0;

			if( !Object.Equals( a, b ) )
			{
				IComparable ic = a as IComparable;
				if( ic != null )
				{
					retVal = ic.CompareTo( b );
				}
				else
				{
					/*
					 * Se non sono IComparable, proviamo
					 * a confrontare la loro rappresentazione
					 * sottoforma di stringa
					 */
					var av = a == null ? ( String )null : a.ToString();
					var bv = b == null ? ( String )null : b.ToString();

					retVal = String.Compare( av, bv, StringComparison.CurrentCulture );
				}
			}

			if( direction == ListSortDirection.Descending )
			{
				retVal *= -1;
			}

			return retVal;
		}

		/// <summary>
		/// Compares two objects and returns a item indicating whether one is less than, equal to, or greater than the other.
		/// </summary>
		/// <param name="x">The first object to compare.</param>
		/// <param name="y">The second object to compare.</param>
		/// <returns>
		/// Value Condition Less than zero<paramref name="x" /> is less than <paramref name="y" />.Zero<paramref name="x" /> equals <paramref name="y" />.Greater than zero<paramref name="x" /> is greater than <paramref name="y" />.
		/// </returns>
		protected virtual Int32 OnCompare( IEntityItemView<T> x, IEntityItemView<T> y )
		{
			/*
			 * Il confronto viene fatto scorrendo tutti
			 * i ListSortDescription...
			 * 
			 * Diventa necessario confrontare i criteri successivi
			 * solo se il precedente non ha dato risultati utili, cioè
			 * i due valori confrontati sono uguali...
			 * 
			 * Il classico esempio è Cognome ASC, Nome DESC confronto i 
			 * nomi solo se i Cognomi sono uguali
			 */
			Int32 retVal = 0;

			if( this.SortDescriptions.Count == 1 )
			{
				ListSortDescription sd = this.SortDescriptions[ 0 ];

				object valueX = sd.PropertyDescriptor.GetValue( x );
				object valueY = sd.PropertyDescriptor.GetValue( y );

				retVal = EntityItemViewSortComparer<T>.Compare( valueX, valueY, sd.SortDirection );
			}
			else
			{
				foreach( ListSortDescription sd in this.SortDescriptions )
				{
					if( retVal == 0 )
					{
						object valueX = sd.PropertyDescriptor.GetValue( x );
						object valueY = sd.PropertyDescriptor.GetValue( y );

						retVal = EntityItemViewSortComparer<T>.Compare( valueX, valueY, sd.SortDirection );
					}
				}
			}
			return retVal;
		}

		/// <summary>
		/// Compares two objects and returns a item indicating whether one is less than, equal to, or greater than the other.
		/// </summary>
		/// <param name="x">The first object to compare.</param>
		/// <param name="y">The second object to compare.</param>
		/// <returns>
		/// Value Condition Less than zero<paramref name="x"/> is less than <paramref name="y"/>.Zero<paramref name="x"/> equals <paramref name="y"/>.Greater than zero<paramref name="x"/> is greater than <paramref name="y"/>.
		/// </returns>
		public Int32 Compare( IEntityItemView<T> x, IEntityItemView<T> y )
		{
			return this.OnCompare( x, y );
		}
	}
}
