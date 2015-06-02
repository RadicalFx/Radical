using System;
using System.Linq;
using System.Reflection;
using Topics.Radical.Validation;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel;
using Topics.Radical.Reflection;
using Castle.Core;

namespace Castle.Windsor
{
	/// <summary>
	/// Custom behaviors for the Castle Windsor container.
	/// </summary>
	public static class ContainerExtensions
	{
		/// <summary>
		/// Determines whether the specified service type T is registered in the container kernel.
		/// </summary>
		/// <typeparam name="T">The type to look for.</typeparam>
		/// <param name="container">The container.</param>
		/// <returns>
		/// 	<c>true</c> if the specified service type T is registered; otherwise, <c>false</c>.
		/// </returns>
		public static Boolean IsRegistered<T>( this IWindsorContainer container )
		{
			Ensure.That( container ).Named( () => container ).IsNotNull();

			var value = container.Kernel.HasComponent( typeof( T ) );

			return value;
		}

		const string OVERRIDABLE_OVERRIDABLE_REGISTRATION = "overridable-component-registration";

		/// <summary>
		/// Determines whether the specified component model is overridable.
		/// </summary>
		/// <param name="componentModel">The component model.</param>
		/// <returns>
		///   <c>true</c> if the specified component model is overridable; otherwise, <c>false</c>.
		/// </returns>
		public static Boolean IsOverridable( this ComponentModel componentModel )
		{
			return componentModel.ExtendedProperties.Contains( OVERRIDABLE_OVERRIDABLE_REGISTRATION );
		}

		/// <summary>
		/// Defines the specified component registration as overridable.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="registration">The registration.</param>
		/// <returns>The same component registration.</returns>
		public static ComponentRegistration<T> Overridable<T>( this ComponentRegistration<T> registration )
            where T : class
		{
			registration.ExtendedProperties
			( 
				Property.ForKey( OVERRIDABLE_OVERRIDABLE_REGISTRATION )
					.Eq( true ) 
			);
			
			return registration;
		}

		/// <summary>
		/// Determines whether the specified container has registered the givent facility.
		/// </summary>
		/// <typeparam name="T">The type of the facility.</typeparam>
		/// <param name="container">The container.</param>
		/// <returns>
		/// 	<c>true</c> if the specified container has registered the given facility; otherwise, <c>false</c>.
		/// </returns>
		public static Boolean HasFacility<T>( this IWindsorContainer container )
			where T : IFacility
		{
			Ensure.That( container ).Named( () => container ).IsNotNull();

			var value = container.Kernel.GetFacilities().Where( f => f.GetType().Is<T>() ).Any();

			return value;
		}
	}
}
