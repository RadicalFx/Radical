using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Topics.Radical.Design;
using System.Globalization;

namespace Topics.Radical.Presentation.DesignTimeData.Design
{
	public class AddressViewModelDesignTimeData : DesignTimeHost<AddressViewModel>
	{ }

    public class PersonViewModelDesignTimeData : DesignTimeHost<PersonViewModel>
    {
        public PersonViewModelDesignTimeData()
        {
            this.Expose( v => v.Localization )
                .AsLocalizableResource()
                .WithStaticValue( new Strings() );

            this.Expose( v => v.FirstName )
                .WithLiveValue( () => this.dt_FirstName );

            this.Expose( v => v.Degreed )
                .WithLiveValue( () => this.dt_Degreed );

            this.Expose( v => v.LastName )
                .WithDynamicValue( ci => DateTime.Now.ToLongTimeString() );// "Servienti" );

			//this.Expose( v => v.Address )
			//	.WithSimilarValue( new AddressViewModelDesignTimeData() );
        }

        //protected override void OnUICultureChanged()
        //{
        //    base.OnUICultureChanged();

        //    Strings.Culture = this.UICulture;

        //    this.OnExposedPropertyChanged( v => v.Localization );
        //}

        public String dt_FirstName
        {
            get { return this.GetPropertyValue( () => this.dt_FirstName ); }
            set { this.SetPropertyValue( () => this.dt_FirstName, value ); }
        }

        public Boolean dt_Degreed
        {
            get { return this.GetPropertyValue( () => this.dt_Degreed ); }
            set { this.SetPropertyValue( () => this.dt_Degreed, value ); }
        }
    }
}
