using System;
using System.IO;
using System.Runtime.Serialization.Json;
using Topics.Radical.Validation;

namespace Topics.Radical.ServiceModel.Web
{
	/// <summary>
	/// Extends all the objects adding wcf (rest) extensions.
	/// </summary>
	public static class ObjectExtensions
	{
		/// <summary>
		/// Serialize the supplied source graph in json format.
		/// </summary>
		/// <typeparam name="T">The type of the source graph.</typeparam>
		/// <param name="source">The source graph.</param>
		/// <returns>A string in json format.</returns>
		public static String AsJson<T>( this T source ) 
		{
			if( Object.ReferenceEquals( null, source ) ) 
			{
				throw new ArgumentNullException( "source" );
			}

			using( var ms = new MemoryStream() )
			{
				var serializer = new DataContractJsonSerializer( typeof( T ) );
				serializer.WriteObject( ms, source );

				ms.Position = 0;

				using( var reader = new StreamReader( ms ) )
				{
					var json = reader.ReadToEnd();

					return json;
				}
			}
		}
	}
}
