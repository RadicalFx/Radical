using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Radical.Samples.Unity2.ComponentModel;

namespace Radical.Samples.Unity2.Services
{
	class Foo : IFoo, IBar
	{
	}
}

namespace Radical.Samples.Unity2.ComponentModel
{
	interface IFoo { }

	interface IBar { }
}
