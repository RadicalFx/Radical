using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Topics.Radical.ComponentModel;
using Topics.Radical.ComponentModel.Messaging;
using Topics.Radical.Linq;
using Topics.Radical.Reflection;

#if !WINDOWS_PHONE_8
using Topics.Radical.Windows.Presentation.ComponentModel.Regions;
#endif

namespace Topics.Radical.Windows.Presentation.Boot
{
	/// <summary>
	/// 
	/// </summary>
	public class BootstrapConventions
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="BootstrapConventions" /> class.
		/// </summary>
		public BootstrapConventions()
		{
			this.IsConcreteType = t => !t.IsInterface && !t.IsAbstract && !t.IsGenericType;

			this.IsService = t => this.IsConcreteType( t ) && t.Namespace.IsLike( "*.Services" );

			this.SelectServiceContracts = type =>
			{
				var types = new HashSet<Type>( type.GetInterfaces() );
				if ( types.None() || type.IsAttributeDefined<ContractAttribute>() )
				{
					types.Add( type );
				}

				return types;
			};

			this.IsMessageHandler = t =>
			{
#pragma warning disable 618
				return t.Namespace != null && t.Namespace.IsLike( "*.Messaging.Handlers" ) && ( t.Is<IMessageHandler>() || t.Is<IHandleMessage>() );
#pragma warning restore 618
			};

			this.SelectMessageHandlerContracts = type => type.GetInterfaces().Take( 1 );

			this.IsViewModel = t => this.IsConcreteType( t ) && t.FullName.IsLike( "*.Presentation.*ViewModel" );

			this.IsShellViewModel = ( services, implementation ) =>
			{
				return services.Any( t => t.Name.IsLike( "Main*" ) || t.Name.IsLike( "Shell*" ) );
			};

			this.SelectViewModelContracts = type => new[] { type };

			this.IsView = t => this.IsConcreteType( t ) && t.FullName.IsLike( "*.Presentation.*View" );

			this.IsShellView = ( services, implementation ) =>
			{
				return services.Any( t => t.Name.IsLike( "Main*" ) || t.Name.IsLike( "Shell*" ) );
			};

			this.SelectViewContracts = type => new[] { type };
#if !WINDOWS_PHONE_8

			this.GetInterestedRegionNameIfAny = type =>
			{
				if ( this.IsView( type ) )
				{

					if ( type.IsAttributeDefined<InjectViewInRegionAttribute>() )
					{
						return type.GetAttribute<InjectViewInRegionAttribute>().Named;
					}
					
					if ( type.Namespace.IsLike( "*.Presentation.Partial.*" ) )
					{
						var regionName = type.Namespace.Split( '.' ).Last();
						return regionName;
					}
				}

				return null;
			};
#endif
			
			this.IsExcluded = t =>
			{
				return t.IsAttributeDefined<DisableAutomaticRegistrationAttribute>();
			};

#if !SILVERLIGHT

			this.AssemblyFileScanPatterns = entryAssembly => 
			{
				var name = entryAssembly.GetName().Name;

				var dllPattern = String.Format( "{0}*.dll", name );
				var radical = "Radical.*.dll";

				return new[] { dllPattern, radical };
			};

			this.IncludeAssemblyInContainerScan = assembly => true;
#endif
		}

		/// <summary>
		/// Gets or sets the type of the is concrete.
		/// </summary>
		/// <value>
		/// The type of the is concrete.
		/// </value>
		public Predicate<Type> IsConcreteType { get; set; }

		/// <summary>
		/// Gets or sets the is service.
		/// </summary>
		/// <value>
		/// The is service.
		/// </value>
		public Predicate<Type> IsService { get; set; }

		/// <summary>
		/// Gets or sets the select service contracts.
		/// </summary>
		/// <value>
		/// The select service contracts.
		/// </value>
		public Func<Type, IEnumerable<Type>> SelectServiceContracts { get; set; }

		/// <summary>
		/// Gets or sets the is message handler.
		/// </summary>
		/// <value>
		/// The is message handler.
		/// </value>
		public Predicate<Type> IsMessageHandler { get; set; }

		/// <summary>
		/// Gets or sets the select message handler contracts.
		/// </summary>
		/// <value>
		/// The select message handler contracts.
		/// </value>
		public Func<Type, IEnumerable<Type>> SelectMessageHandlerContracts { get; set; }

		/// <summary>
		/// Gets or sets the is view.
		/// </summary>
		/// <value>
		/// The is view.
		/// </value>
		public Predicate<Type> IsView { get; set; }

		/// <summary>
		/// Gets or sets the is view model.
		/// </summary>
		/// <value>
		/// The is view model.
		/// </value>
		public Predicate<Type> IsViewModel { get; set; }

		/// <summary>
		/// Gets or sets the is shell view.
		/// </summary>
		/// <value>
		/// The is shell view.
		/// </value>
		public Func<IEnumerable<Type>, Type, Boolean> IsShellView { get; set; }

		/// <summary>
		/// Gets or sets the is shell view model.
		/// </summary>
		/// <value>
		/// The is shell view model.
		/// </value>
		public Func<IEnumerable<Type>, Type, Boolean> IsShellViewModel { get; set; }

		/// <summary>
		/// Gets or sets the select view contracts.
		/// </summary>
		/// <value>
		/// The select view contracts.
		/// </value>
		public Func<Type, IEnumerable<Type>> SelectViewContracts { get; set; }

		/// <summary>
		/// Gets or sets the select view model contracts.
		/// </summary>
		/// <value>
		/// The select view model contracts.
		/// </value>
		public Func<Type, IEnumerable<Type>> SelectViewModelContracts { get; set; }

#if !WINDOWS_PHONE_8
		/// <summary>
		/// Gets or sets the get interested region name if any.
		/// </summary>
		/// <value>
		/// The get interested region name if any.
		/// </value>
		public Func<Type, String> GetInterestedRegionNameIfAny { get; set; }
#endif

		/// <summary>
		/// Gets or sets the is excluded.
		/// </summary>
		/// <value>
		/// The is excluded.
		/// </value>
		public Func<Type, Boolean> IsExcluded { get; set; }

#if !SILVERLIGHT

		/// <summary>
		/// Gets or sets the assembly file scan patterns.
		/// </summary>
		/// <value>
		/// The assembly file scan patterns.
		/// </value>
		public Func<Assembly, IEnumerable<String>> AssemblyFileScanPatterns{get;set;}

		/// <summary>
		/// Gets or sets the include assembly in container scan.
		/// </summary>
		/// <value>
		/// The include assembly in container scan.
		/// </value>
		public Predicate<Assembly> IncludeAssemblyInContainerScan { get; set; }

#endif
	}
}
