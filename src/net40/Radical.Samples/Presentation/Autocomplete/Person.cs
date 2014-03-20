using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Topics.Radical.Windows.Behaviors;

namespace Topics.Radical.Presentation.Autocomplete
{
	public class Person //: 
		//AutoComplete.ICanRepresentMyself,
		//AutoComplete.IHaveAnOpinionOnFilter
	{
		public String FirstName { get; set; }
		public String LastName { get; set; }

		public String FullName { get { return this.FirstName + " " + this.LastName; } }

		public override string ToString()
		{
			return this.FirstName;
		}

		//string AutoComplete.ICanRepresentMyself.AsString()
		//{
		//	return this.FullName;
		//}

		//bool AutoComplete.IHaveAnOpinionOnFilter.Match( string userText )
		//{
		//	return this.FullName.StartsWith( userText, StringComparison.OrdinalIgnoreCase );
		//}
	}
}
