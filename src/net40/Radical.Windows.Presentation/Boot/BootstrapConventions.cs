using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Topics.Radical.ComponentModel;
using Topics.Radical.ComponentModel.Messaging;
using Topics.Radical.Linq;
using Topics.Radical.Reflection;
using Topics.Radical.Windows.Presentation.ComponentModel;

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
			this.DefaultIsConcreteType = t => !t.IsInterface && !t.IsAbstract && !t.IsGenericType;
			this.IsConcreteType = t => this.DefaultIsConcreteType( t );

			this.DefaultIsService = t => this.IsConcreteType( t ) && t.Namespace.IsLike( "*.Services" );
			this.IsService = t => this.DefaultIsService( t );

			this.DefaultSelectServiceContracts = type =>
			{
				var types = new HashSet<Type>( type.GetInterfaces() );
				if( types.None() || type.IsAttributeDefined<ContractAttribute>() )
				{
					types.Add( type );
				}

				return types;
			};
			this.SelectServiceContracts = type => this.DefaultSelectServiceContracts( type );

			this.DefaultIsMessageHandler = t =>
			{
#pragma warning disable 618
				return t.Namespace != null && t.Namespace.IsLike( "*.Messaging.Handlers" ) && ( t.Is<IMessageHandler>() || t.Is<IHandleMessage>() );
#pragma warning restore 618
			};
			this.IsMessageHandler = t => this.DefaultIsMessageHandler( t );

			this.DefaultSelectMessageHandlerContracts = type => type.GetInterfaces().Take( 1 );
			this.SelectMessageHandlerContracts = type => this.DefaultSelectMessageHandlerContracts( type );

			this.DefaultIsViewModel = t => this.IsConcreteType( t ) && t.FullName.IsLike( "*.Presentation.*ViewModel" );
			this.IsViewModel = t => this.DefaultIsViewModel( t );

			this.DefaultIsShellViewModel = ( services, implementation ) =>
			{
				return services.Any( t => t.Name.IsLike( "Main*" ) || t.Name.IsLike( "Shell*" ) );
			};
			this.IsShellViewModel = ( services, implementation ) => this.DefaultIsShellViewModel( services, implementation );

			this.DefaultSelectViewModelContracts = type => new[] { type };
			this.SelectViewModelContracts = type => this.DefaultSelectViewModelContracts( type );

			this.DefaultIsView = t => this.IsConcreteType( t ) && t.FullName.IsLike( "*.Presentation.*View" );
			this.IsView = t => this.DefaultIsView( t );

			this.DefaultIsShellView = ( services, implementation ) =>
			{
				return services.Any( t => t.Name.IsLike( "Main*" ) || t.Name.IsLike( "Shell*" ) );
			};
			this.IsShellView = ( services, implementation ) => this.DefaultIsShellView( services, implementation );

			this.DefaultSelectViewContracts = type => new[] { type };
			this.SelectViewContracts = type => this.DefaultSelectViewContracts( type );
#if !WINDOWS_PHONE_8

			this.DefaultGetInterestedRegionNameIfAny = type =>
			{
				if( this.IsView( type ) )
				{

					if( type.IsAttributeDefined<InjectViewInRegionAttribute>() )
					{
						return type.GetAttribute<InjectViewInRegionAttribute>().Named;
					}

					if( type.Namespace.IsLike( "*.Presentation.Partial.*" ) )
					{
						var regionName = type.Namespace.Split( '.' ).Last();
						return regionName;
					}
				}

				return null;
			};
			this.GetInterestedRegionNameIfAny = type => this.DefaultGetInterestedRegionNameIfAny( type );
#endif

			this.DefaultIsExcluded = t =>
			{
				return t.IsAttributeDefined<DisableAutomaticRegistrationAttribute>();
			};
			this.IsExcluded = t => this.DefaultIsExcluded( t );

#if !SILVERLIGHT

			this.DefaultAssemblyFileScanPatterns = entryAssembly =>
			{
				var name = entryAssembly.GetName().Name;

				var dllPattern = String.Format( "{0}*.dll", name );
				var radical = "Radical.*.dll";

				return new[] { dllPattern, radical };
			};
			this.AssemblyFileScanPatterns = entryAssembly => this.DefaultAssemblyFileScanPatterns( entryAssembly );

			this.DefaultIncludeAssemblyInContainerScan = assembly => true;
			this.IncludeAssemblyInContainerScan = assembly => this.DefaultIncludeAssemblyInContainerScan(assembly);

			this.DefaultIgnorePropertyInjection = pi =>
			{
				var isDefined = pi.IsAttributeDefined<IgnorePropertyInjectionAttribue>();
				return isDefined;
			};
			this.IgnorePropertyInjection = pi => this.DefaultIgnorePropertyInjection( pi );

			this.DefaultIgnoreViewPropertyInjection = pi =>
			{
				return true;
			};
			this.IgnoreViewPropertyInjection = pi => this.DefaultIgnoreViewPropertyInjection( pi );

			this.DefaultIgnoreViewModelPropertyInjection = pi =>
			{
				return true;
			};
			this.IgnoreViewModelPropertyInjection = pi => this.DefaultIgnoreViewModelPropertyInjection( pi );
#endif
		}

		/// <summary>
		/// Default: Gets or sets the type of the is concrete.
		/// </summary>
		/// <value>
		/// The type of the is concrete.
		/// </value>
		[IgnorePropertyInjectionAttribue]
		public Predicate<Type> DefaultIsConcreteType { get; private set; }

		/// <summary>
		/// Gets or sets the type of the is concrete.
		/// </summary>
		/// <value>
		/// The type of the is concrete.
		/// </value>
		[IgnorePropertyInjectionAttribue]
		public Predicate<Type> IsConcreteType { get; set; }

		/// <summary>
		/// Default: Gets or sets the is service.
		/// </summary>
		/// <value>
		/// The is service.
		/// </value>
		[IgnorePropertyInjectionAttribue]
		public Predicate<Type> DefaultIsService { get;private set; }

		/// <summary>
		/// Gets or sets the is service.
		/// </summary>
		/// <value>
		/// The is service.
		/// </value>
		[IgnorePropertyInjectionAttribue]
		public Predicate<Type> IsService { get; set; }

		/// <summary>
		/// Default: Gets or sets the select service contracts.
		/// </summary>
		/// <value>
		/// The select service contracts.
		/// </value>
		[IgnorePropertyInjectionAttribue]
		public Func<Type, IEnumerable<Type>> DefaultSelectServiceContracts { get; private set; }

		/// <summary>
		/// Gets or sets the select service contracts.
		/// </summary>
		/// <value>
		/// The select service contracts.
		/// </value>
		[IgnorePropertyInjectionAttribue]
		public Func<Type, IEnumerable<Type>> SelectServiceContracts { get; set; }

		/// <summary>
		/// Default: Gets or sets the is message handler.
		/// </summary>
		/// <value>
		/// The is message handler.
		/// </value>
		[IgnorePropertyInjectionAttribue]
		public Predicate<Type> DefaultIsMessageHandler { get; private set; }

		/// <summary>
		/// Gets or sets the is message handler.
		/// </summary>
		/// <value>
		/// The is message handler.
		/// </value>
		[IgnorePropertyInjectionAttribue]
		public Predicate<Type> IsMessageHandler { get; set; }

		/// <summary>
		/// Default: Gets or sets the select message handler contracts.
		/// </summary>
		/// <value>
		/// The select message handler contracts.
		/// </value>
		[IgnorePropertyInjectionAttribue]
		public Func<Type, IEnumerable<Type>> DefaultSelectMessageHandlerContracts { get; private set; }

		/// <summary>
		/// Gets or sets the select message handler contracts.
		/// </summary>
		/// <value>
		/// The select message handler contracts.
		/// </value>
		[IgnorePropertyInjectionAttribue]
		public Func<Type, IEnumerable<Type>> SelectMessageHandlerContracts { get; set; }

		/// <summary>
		/// Default: Gets or sets the is view.
		/// </summary>
		/// <value>
		/// The is view.
		/// </value>
		[IgnorePropertyInjectionAttribue]
		public Predicate<Type> DefaultIsView { get; private set; }

		/// <summary>
		/// Gets or sets the is view.
		/// </summary>
		/// <value>
		/// The is view.
		/// </value>
		[IgnorePropertyInjectionAttribue]
		public Predicate<Type> IsView { get; set; }

		/// <summary>
		/// Default: Gets or sets the is view model.
		/// </summary>
		/// <value>
		/// The is view model.
		/// </value>
		[IgnorePropertyInjectionAttribue]
		public Predicate<Type> DefaultIsViewModel { get; private set; }

		/// <summary>
		/// Gets or sets the is view model.
		/// </summary>
		/// <value>
		/// The is view model.
		/// </value>
		[IgnorePropertyInjectionAttribue]
		public Predicate<Type> IsViewModel { get; set; }

		/// <summary>
		/// Default: Gets or sets the is shell view.
		/// </summary>
		/// <value>
		/// The is shell view.
		/// </value>
		[IgnorePropertyInjectionAttribue]
		public Func<IEnumerable<Type>, Type, Boolean> DefaultIsShellView { get; private set; }

		/// <summary>
		/// Gets or sets the is shell view.
		/// </summary>
		/// <value>
		/// The is shell view.
		/// </value>
		[IgnorePropertyInjectionAttribue]
		public Func<IEnumerable<Type>, Type, Boolean> IsShellView { get; set; }

		/// <summary>
		/// Default: Gets or sets the is shell view model.
		/// </summary>
		/// <value>
		/// The is shell view model.
		/// </value>
		[IgnorePropertyInjectionAttribue]
		public Func<IEnumerable<Type>, Type, Boolean> DefaultIsShellViewModel { get; private set; }

		/// <summary>
		/// Gets or sets the is shell view model.
		/// </summary>
		/// <value>
		/// The is shell view model.
		/// </value>
		[IgnorePropertyInjectionAttribue]
		public Func<IEnumerable<Type>, Type, Boolean> IsShellViewModel { get; set; }

		/// <summary>
		/// Default: Gets or sets the select view contracts.
		/// </summary>
		/// <value>
		/// The select view contracts.
		/// </value>
		[IgnorePropertyInjectionAttribue]
		public Func<Type, IEnumerable<Type>> DefaultSelectViewContracts { get; private set; }

		/// <summary>
		/// Gets or sets the select view contracts.
		/// </summary>
		/// <value>
		/// The select view contracts.
		/// </value>
		[IgnorePropertyInjectionAttribue]
		public Func<Type, IEnumerable<Type>> SelectViewContracts { get; set; }

		/// <summary>
		/// Default: Gets or sets the select view model contracts.
		/// </summary>
		/// <value>
		/// The select view model contracts.
		/// </value>
		[IgnorePropertyInjectionAttribue]
		public Func<Type, IEnumerable<Type>> DefaultSelectViewModelContracts { get; private set; }

		/// <summary>
		/// Gets or sets the select view model contracts.
		/// </summary>
		/// <value>
		/// The select view model contracts.
		/// </value>
		[IgnorePropertyInjectionAttribue]
		public Func<Type, IEnumerable<Type>> SelectViewModelContracts { get; set; }

#if !WINDOWS_PHONE_8
		/// <summary>
		/// Default: Gets or sets the get interested region name if any.
		/// </summary>
		/// <value>
		/// The get interested region name if any.
		/// </value>
		[IgnorePropertyInjectionAttribue]
		public Func<Type, String> DefaultGetInterestedRegionNameIfAny { get; private set; }

		/// <summary>
		/// Gets or sets the get interested region name if any.
		/// </summary>
		/// <value>
		/// The get interested region name if any.
		/// </value>
		[IgnorePropertyInjectionAttribue]
		public Func<Type, String> GetInterestedRegionNameIfAny { get; set; }
#endif

		/// <summary>
		/// Default: Gets or sets the is excluded.
		/// </summary>
		/// <value>
		/// The is excluded.
		/// </value>
		[IgnorePropertyInjectionAttribue]
		public Func<Type, Boolean> DefaultIsExcluded { get; private set; }

		/// <summary>
		/// Gets or sets the is excluded.
		/// </summary>
		/// <value>
		/// The is excluded.
		/// </value>
		[IgnorePropertyInjectionAttribue]
		public Func<Type, Boolean> IsExcluded { get; set; }

#if !SILVERLIGHT

		/// <summary>
		/// Default: Gets or sets the assembly file scan patterns.
		/// </summary>
		/// <value>
		/// The assembly file scan patterns.
		/// </value>
		[IgnorePropertyInjectionAttribue]
		public Func<Assembly, IEnumerable<String>> DefaultAssemblyFileScanPatterns { get; private set; }

		/// <summary>
		/// Gets or sets the assembly file scan patterns.
		/// </summary>
		/// <value>
		/// The assembly file scan patterns.
		/// </value>
		[IgnorePropertyInjectionAttribue]
		public Func<Assembly, IEnumerable<String>> AssemblyFileScanPatterns { get; set; }

		/// <summary>
		/// Default: Gets or sets the include assembly in container scan.
		/// </summary>
		/// <value>
		/// The include assembly in container scan.
		/// </value>
		[IgnorePropertyInjectionAttribue]
		public Predicate<Assembly> DefaultIncludeAssemblyInContainerScan { get; private set; }

		/// <summary>
		/// Gets or sets the include assembly in container scan.
		/// </summary>
		/// <value>
		/// The include assembly in container scan.
		/// </value>
		[IgnorePropertyInjectionAttribue]
		public Predicate<Assembly> IncludeAssemblyInContainerScan { get; set; }

		/// <summary>
		/// Default: Gets or sets the predicate that determines if a property is injectable or not.
		/// </summary>
		/// <value>
		/// The injectable properties predicate.
		/// </value>
		[IgnorePropertyInjectionAttribue]
		public Func<PropertyInfo, Boolean> DefaultIgnorePropertyInjection { get; private set; }

		/// <summary>
		/// Gets or sets the predicate that determines if a property is injectable or not.
		/// </summary>
		/// <value>
		/// The injectable properties predicate.
		/// </value>
		[IgnorePropertyInjectionAttribue]
		public Func<PropertyInfo, Boolean> IgnorePropertyInjection { get; set; }

		/// <summary>
		/// Default: Gets or sets the predicate that determines if a property of a View is injectable or not.
		/// </summary>
		/// <value>
		/// The injectable properties predicate.
		/// </value>
		[IgnorePropertyInjectionAttribue]
		public Func<PropertyInfo, Boolean> DefaultIgnoreViewPropertyInjection { get; private set; }

		/// <summary>
		/// Gets or sets the predicate that determines if a property of a View is injectable or not.
		/// </summary>
		/// <value>
		/// The injectable properties predicate.
		/// </value>
		[IgnorePropertyInjectionAttribue]
		public Func<PropertyInfo, Boolean> IgnoreViewPropertyInjection { get; set; }

		/// <summary>
		/// Default: Gets or sets the predicate that determines if a property of a ViewModel is injectable or not.
		/// </summary>
		/// <value>
		/// The injectable properties predicate.
		/// </value>
		[IgnorePropertyInjectionAttribue]
		public Func<PropertyInfo, Boolean> DefaultIgnoreViewModelPropertyInjection { get; private set; }

		/// <summary>
		/// Gets or sets the predicate that determines if a property of a ViewModel is injectable or not.
		/// </summary>
		/// <value>
		/// The injectable properties predicate.
		/// </value>
		[IgnorePropertyInjectionAttribue]
		public Func<PropertyInfo, Boolean> IgnoreViewModelPropertyInjection { get; set; }

#endif
	}
}
