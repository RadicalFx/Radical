using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Topics.Radical.Windows.Presentation.ComponentModel
{
	public interface INavigationScopedStorage
	{
		Object GetData( String key );

		T GetData<T>( String key );

		void SetData( String key, Object data, StorageLocation location );

		void RemoveData( String key );

		Boolean Contains( String key );

		void Clear();
	}
}
