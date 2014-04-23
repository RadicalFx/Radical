using System;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;


namespace TestGridView.Model
{
	class CLIENTI : INotifyPropertyChanged
	{
		private int id;
		private string name;

		public CLIENTI( int ID, string Name )
		{
			this.ID = ID;
			this.Name = Name;
		}


		public int ID
		{
			get
			{
				return id;
			}
			set
			{
				id = value;
				OnPropertyChanged( () => ID );
			}
		}

		public string Name
		{
			get
			{
				return name;
			}
			set
			{
				name = value;
				OnPropertyChanged( () => Name );
			}
		}

		#region INotifyPropertyChanged Members

		public void OnPropertyChanged( string p )
		{
			if ( PropertyChanged != null )
			{
				PropertyChanged( this, new PropertyChangedEventArgs( p ) );
			}
		}

		protected virtual void OnPropertyChanged<T>( Expression<Func<T>> selectorExpression )
		{
			MemberExpression body = selectorExpression.Body as MemberExpression;
			OnPropertyChanged( body.Member.Name );
		}

		public event PropertyChangedEventHandler PropertyChanged;

		#endregion
	}
}
