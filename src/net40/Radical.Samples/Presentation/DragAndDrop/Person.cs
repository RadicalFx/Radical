using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Topics.Radical.Presentation.DragAndDrop
{
	class Person
	{
		public String FirstName { get; set; }
		public String LastName { get; set; }

		public override string ToString()
		{
			return this.FirstName + " " + this.LastName;
		}
	}
}
