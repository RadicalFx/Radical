using System;
using Topics.Radical.ComponentModel;
using Topics.Radical.Conversions;

namespace Topics.Radical.Model.Services
{
	public class Int32KeyService : IKeyService
	{
		public IKey GenerateEmpty()
		{
			return ( 0 ).AsKey();
		}
	}
}
