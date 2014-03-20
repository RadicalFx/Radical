using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topics.Radical.Model;

namespace Topics.Radical.Presentation.DesignTimeData
{
	public class AddressViewModel : Entity
	{
		public String City
		{
			get { return this.GetPropertyValue( () => this.City ); }
			set { this.SetPropertyValue( () => this.City, value ); }
		}
	}
}
