namespace Topics.Radical.Model
{
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using Topics.Radical.ComponentModel;

	sealed class DefaultEntityItemViewSortComparer<T> :
		IComparer<IEntityItemView<T>>
		//where T : class
	{
		IList dataSource;

		public DefaultEntityItemViewSortComparer( IList dataSource )
		{
			this.dataSource = dataSource;
		}

		#region IComparer<IEntityItemView<T>> Members

		public int Compare( IEntityItemView<T> x, IEntityItemView<T> y )
		{
			/*
			 * Questo comparer ha lo scopo di ordinare gli item come sono 
			 * nella data source... quindi per prima cosa recuperiamo l'indice
			 * nella DataSource l'implementazione di default del confronto
			 * fra indici ci va benissimo tranne per il fatto è che presumibile
			 * che un EntityItem non esista ancora nella DataSource perchè non
			 * è stato ancora "Committed" in qusto caso il compare di default
			 * ce lo metterebbe in testa perchè l'indice -1 è minore... ergo
			 * dobbiamo gestire questo "special case"
			 */
			Int32 xIndex = this.dataSource.IndexOf( x.EntityItem );
			Int32 yIndex = this.dataSource.IndexOf( y.EntityItem );

			if( xIndex == -1 && yIndex != -1 )
			{
				/*
				 * x non c'è, lo dobbiamo mettere comunque in fondo
				 * --> x è più grande di y
				 */
				return -1;
			}
			else if( xIndex != -1 && yIndex == -1 )
			{
				/*
				 * y non c'è, lo dobbiamo mettere comunque in fondo
				 * --> x è più grande di y
				 */
				return 1;
			}
			else
			{
				return xIndex.CompareTo( yIndex );
			}
		}

		#endregion
	}
}
