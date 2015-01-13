using System;
using Topics.Radical.Model;

namespace Topics.Radical.Presentation.Memento.ComplexGraph
{
	public class AddressViewModel : MementoEntity
	{
		public void Initialize( Address address, Boolean registerAsTransient )
		{
			if( registerAsTransient )
			{
				this.RegisterTransient();
			}

			this.SetInitialPropertyValue
			(
				() => this.Street,
				address.With( a => a.Street ).Return( s => s, "" )
			);

			this.SetInitialPropertyValue
			(
				() => this.Number,
				address.With( a => a.Number ).Return( n => n, "" )
			);

			this.SetInitialPropertyValue
			(
				() => this.City,
				address.With( a => a.City ).Return( c => c, "" )
			);

			this.SetInitialPropertyValue
			(
				() => this.ZipCode,
				address.With( a => a.ZipCode ).Return( zc => zc, "" )
			);
		}

		public String Street
		{
			get { return this.GetPropertyValue( () => this.Street ); }
			set { this.SetPropertyValue( () => this.Street, value ); }
		}

		public String Number
		{
			get { return this.GetPropertyValue( () => this.Number ); }
			set { this.SetPropertyValue( () => this.Number, value ); }
		}

		public String City
		{
			get { return this.GetPropertyValue( () => this.City ); }
			set { this.SetPropertyValue( () => this.City, value ); }
		}

		public String ZipCode
		{
			get { return this.GetPropertyValue( () => this.ZipCode ); }
			set { this.SetPropertyValue( () => this.ZipCode, value ); }
		}
	}
}
