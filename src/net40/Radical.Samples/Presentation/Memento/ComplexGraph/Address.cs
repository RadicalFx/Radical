using System;
using Topics.Radical.Validation;

namespace Topics.Radical.Presentation.Memento.ComplexGraph
{
	public class Address
	{
		public Address( Person owner )
		{
			Ensure.That( owner ).Named( () => owner ).IsNotNull();

			this.Person = owner;
		}

		public Person Person { get; private set; }

		public String Street { get; set; }
		public String Number { get; set; }
		public String City { get; set; }
		public String ZipCode { get; set; }
	}
}
