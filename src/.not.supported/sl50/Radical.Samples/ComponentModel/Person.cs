using System;
using System.Collections.Generic;

namespace Topics.Radical.ComponentModel
{
	public class Person
	{
		public String FirstName { get; set; }
		public String LastName { get; set; }

		public IList<Address> Addresses { get; private set; }
	}
}
