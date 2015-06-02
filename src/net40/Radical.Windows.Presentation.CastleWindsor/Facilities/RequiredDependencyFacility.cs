using System;
using System.Linq;
using Castle.MicroKernel.Facilities;
using System.Reflection;
using System.Text;
using System.Collections.Generic;

namespace Castle.Facilities
{
	/// <summary>
	/// A Windsor facility that intercepts a component creation
	/// and determines if all the required dependencies are satisfied
	/// or not.
	/// </summary>
	public sealed class RequiredDependencyFacility : AbstractFacility
	{
		/// <summary>
		/// The custom initialization for the Facility.
		/// </summary>
		/// <remarks>It must be overriden.</remarks>
		protected override void Init()
		{
			this.Kernel.ComponentCreated += ( s, e ) =>
			{
				if( e != null )
				{
					var requiredNotWired = e.GetRequiredNotWired();
					if( requiredNotWired.Any() )
					{
						var sb = new StringBuilder()
							.AppendFormat( "An error occured during dependency resolution of '{0}' component.", e.GetType().Name )
							.AppendLine()
							.AppendLine( "Review the following errors for details:" );

						requiredNotWired.Aggregate( sb, ( builder, p ) =>
						{
							return builder
								.AppendFormat( " - Property '{0}' depends on '{1}' type, that cannot be resolved.", p.Name, p.PropertyType.Name )
								.AppendLine();
						} );

						throw new MissingRequiredDependencyException( sb.ToString() );
					}
				}
			};
		}
	}

	static class PropertyInfoExtension
	{
		public static Boolean IsRequired( this MemberInfo property )
		{
			var attributes = property.GetCustomAttributes( typeof( RequiredDependencyAttribute ), false );
			return attributes != null && attributes.Length == 1;
		}

		public static IEnumerable<PropertyInfo> GetRequiredNotWired( this Object component )
		{
			return component.GetType()
				.GetProperties()
				.Where( pi => pi.IsRequired() && pi.GetValue( component, null ) == null );
		}
	}
}