using System;
using System.Collections.Generic;

namespace Topics.Radical.Presentation.Memento.ComplexGraph
{
	public class Person
	{
		public Person()
		{
			this.Addresses = new List<Address>();
		}

		public String FirstName { get; set; }
		public String LastName { get; set; }

		public IList<Address> Addresses { get; private set; }
	}
}
