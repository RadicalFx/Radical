using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Topics.Radical.Validation;
using Windows.Storage;

namespace Topics.Radical.Windows.Services
{
	class StorageSerializationHelpers
	{
		public async Task<T> DeserializeFromFileAsync<T>( StorageFile file )
		{
			Ensure.That( file ).Named( () => file ).IsNotNull();

			using( var fs = ( await file.OpenReadAsync() ).AsStreamForRead() )
			{
				using( var ms = new MemoryStream() )
				{
					await fs.CopyToAsync( ms );
					ms.Position= 0;

					var serializer = new DataContractSerializer( typeof( T ) );
					return ( T )serializer.ReadObject( ms );
				}
			}
		}

		public async Task<T> DeserializeFromFileAsync<T>( StorageFolder folder, string fileName )
		{
			Ensure.That( folder ).Named( () => folder ).IsNotNull();
			Ensure.That( fileName ).Named( () => fileName ).IsNotNullNorEmpty();

			var file = await folder.GetFileAsync( fileName );
			return await DeserializeFromFileAsync<T>( file );
		}

		public async Task SerializeToFileAsync<T>( StorageFile file, T value )
		{
			Ensure.That( file ).Named( () => file ).IsNotNull();

			using( var ms = new MemoryStream() )
			{
				var serializer = new DataContractSerializer( typeof( T ) );
				serializer.WriteObject( ms, value );

				using( var fileStream = await file.OpenStreamForWriteAsync() )
				{
					ms.Seek( 0, SeekOrigin.Begin );
					await ms.CopyToAsync( fileStream );
					await fileStream.FlushAsync();
				}
			}
		}

		public async Task SerializeToFileAsync<T>( StorageFolder folder, String fileName, T value )
		{
			Ensure.That( folder ).Named( () => folder ).IsNotNull();
			Ensure.That( fileName ).Named( () => fileName ).IsNotNullNorEmpty();
			
			var file = await folder.CreateFileAsync( fileName, CreationCollisionOption.ReplaceExisting );
			await SerializeToFileAsync<T>( file, value );
		}
	}
}
