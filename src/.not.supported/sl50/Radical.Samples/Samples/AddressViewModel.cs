using System;
using Topics.Radical.ComponentModel;
using Topics.Radical.Model;

namespace Topics.Radical.Samples
{
	public class AddressViewModel : MementoEntity
	{
		public void Initialize( Address address, Boolean registerAsTransient )
		{
			if( registerAsTransient )
			{
				this.RegisterTransient();
			}

			this.SetInitialPropertyValue( () => this.Street, address.Street );
			this.SetInitialPropertyValue( () => this.Number, address.Number );
			this.SetInitialPropertyValue( () => this.City, address.City );
			this.SetInitialPropertyValue( () => this.ZipCode, address.ZipCode );
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
