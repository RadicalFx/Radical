using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.ObjectBuilder;
using Topics.Radical.Validation;

namespace Topics.Radical.Windows.Presentation.Extensions
{
	public class CandidateConstructorSelector : InjectionMember, IConstructorSelectorPolicy
	{
		readonly IUnityContainer container;
		Type implementationType;

		public CandidateConstructorSelector( IUnityContainer container )
		{
			this.container = container;
		}

		public override void AddPolicies( Type serviceType, Type implementationType, string name, IPolicyList policies )
		{
			this.implementationType = implementationType;

			policies.Set<IConstructorSelectorPolicy>( this, new NamedTypeBuildKey( implementationType, name ) );
		}

		//Boolean IsRegistered( IBuilderContext context, Type key ) 
		//{
		//	var policy = context.PersistentPolicies.Get<IBuildKeyMappingPolicy>( key );

		//	return policy != null;
		//}

		public SelectedConstructor SelectConstructor( IBuilderContext context, IPolicyList resolverPolicyDestination )
		{
			var ctor = this.implementationType.GetConstructors()
				.FirstOrDefault( x =>
				{
					return !x.IsStatic && x.GetParameters()
						.All( y =>
						{
							var pti = y.ParameterType;

							//if ( entry.Parameters.ContainsKey( y.Name ) && pti.IsAssignableFrom( entry.Parameters[ y.Name ].GetType() ) )
							//{
							//	return true;
							//}


							//if ( this.IsRegistered( context, pti ) )
							//{
							//	return true;
							//}
							if ( this.container.IsRegistered( pti ) )
							{
								return true;
							}

							if ( pti.IsArray && pti.HasElementType )
							{
								var eti = pti.GetElementType();

								return this.container.IsRegistered( eti );

								//return this.IsRegistered( context, eti );
							}

							if ( pti.IsGenericType )
							{
								//is "IEnumerable"
							}

							return false;
						} );
				} );

			Ensure.That( ctor )
					.Named( "ctor" )
					.WithMessage( "Cannot find any valid constructor fot type {0}.", implementationType.FullName )
					.IsNotNull();

			//var parameterValues = new List<Object>();
			
			var values = ctor.GetParameters()
				.Aggregate( new List<Object>(), ( objs, p ) =>
				{
					objs.Add( p.ParameterType );
					return objs;
				} )
				.Select( o => InjectionParameterValue.ToParameter( o ) )
				.ToArray();

			//.ForEach( p =>
			//{
			//	parameterValues.Add( p.ParameterType );
			//} );

			//var values = InjectionParameterValue.ToParameters( parameterValues.ToArray() );

			var selectedCtor = new SelectedConstructor( ctor );
			SpecifiedMemberSelectorHelper.AddParameterResolvers( this.implementationType, resolverPolicyDestination, values, selectedCtor );

			return selectedCtor;
		}
	}
}
