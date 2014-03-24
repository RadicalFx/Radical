using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Topics.Radical.ComponentModel
{
	public class Person
	{
		public Person()
			: this( String.Empty )
		{

		}

		public Person( String username )
		{
			this.Username = username;
			this.Addresses = new List<Address>();
		}

		public String Username { get; private set; }

		public String FirstName { get; set; }
		public String LastName { get; set; }

		[Bindable( BindableSupport.No )]
		public IList<Address> Addresses { get; private set; }
	}
}
