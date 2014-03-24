using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Topics.Radical.ComponentModel
{
	public interface IPuzzleSetupDescriptor
	{
		Task Setup( IPuzzleContainer container, Func<IEnumerable<TypeInfo>> knownTypesProvider );
	}
}
