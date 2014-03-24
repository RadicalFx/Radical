using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Topics.Radical.Windows.Presentation.ComponentModel
{
	public interface ISuspensionManager
	{
		Object GetValue( String key );

		T GetValue<T>( String key );

		void SetValue( String key, Object data, StorageLocation location );

		void Remove( String key );

		Boolean Contains( String key );

		Task SuspendAsync();

		Task ResumeAsync();
	}
}
