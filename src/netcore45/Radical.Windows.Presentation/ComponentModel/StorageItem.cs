using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Topics.Radical.Windows.Presentation.ComponentModel
{
	[DataContract]
	class StorageItem
	{
		[DataMember]
		public StorageLocation Location { get; set; }

		[DataMember]
		public Object Data { get; set; }
	}
}
