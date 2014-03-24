using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Markup;

namespace Topics.Radical.Windows.Markup
{
	/// <summary>
	/// A base class for custom markup extension which provides properties
	/// that can be found on regular <see cref="Binding"/> markup extension.
	/// </summary>
#if !SILVERLIGHT
	[MarkupExtensionReturnType( typeof( object ) )]
#endif
	public abstract class BindingDecoratorBase : MarkupExtension
	{
		/// <summary>
		/// The decorated binding class.
		/// </summary>
		private Binding binding = null;

		/// <summary>
		/// Initializes a new instance of the <see cref="BindingDecoratorBase"/> class.
		/// </summary>
		protected BindingDecoratorBase()
		{
			binding = new Binding();
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="BindingDecoratorBase"/> class.
		/// </summary>
		/// <param name="path">The path.</param>
		protected BindingDecoratorBase( String path )
		{
			binding = new Binding( path );
		}

		/// <summary>
		/// The decorated binding class.
		/// </summary>
		[Browsable( false )]
		public Binding Binding
		{
			get { return binding; }
			set { binding = value; }
		}

#if !SILVERLIGHT

		[DefaultValue( null )]
		public object AsyncState
		{
			get { return binding.AsyncState; }
			set { binding.AsyncState = value; }
		}

#endif

		[DefaultValue( false )]
		public bool BindsDirectlyToSource
		{
			get { return binding.BindsDirectlyToSource; }
			set { binding.BindsDirectlyToSource = value; }
		}

		[DefaultValue( null )]
		public IValueConverter Converter
		{
			get { return binding.Converter; }
			set { binding.Converter = value; }
		}

#if !SILVERLIGHT
		[TypeConverter( typeof( CultureInfoIetfLanguageTagConverter ) ), DefaultValue( null )]
#endif
		public CultureInfo ConverterCulture
		{
			get { return binding.ConverterCulture; }
			set { binding.ConverterCulture = value; }
		}

		[DefaultValue( null )]
		public object ConverterParameter
		{
			get { return binding.ConverterParameter; }
			set { binding.ConverterParameter = value; }
		}

		[DefaultValue( null )]
		public string ElementName
		{
			get { return binding.ElementName; }
			set { binding.ElementName = value; }
		}

		[DefaultValue( null )]
		public object FallbackValue
		{
			get { return binding.FallbackValue; }
			set { binding.FallbackValue = value; }
		}

#if !SILVERLIGHT
		[DefaultValue( false )]
		public bool IsAsync
		{
			get { return binding.IsAsync; }
			set { binding.IsAsync = value; }
		}
#endif
#if !SILVERLIGHT
		[DefaultValue( BindingMode.Default )]
#else
		[DefaultValue( BindingMode.TwoWay )]
#endif
		public BindingMode Mode
		{
			get { return binding.Mode; }
			set { binding.Mode = value; }
		}

#if !SILVERLIGHT
		[DefaultValue( false )]
		public bool NotifyOnSourceUpdated
		{
			get { return binding.NotifyOnSourceUpdated; }
			set { binding.NotifyOnSourceUpdated = value; }
		}

		[DefaultValue( false )]
		public bool NotifyOnTargetUpdated
		{
			get { return binding.NotifyOnTargetUpdated; }
			set { binding.NotifyOnTargetUpdated = value; }
		}
#endif

		[DefaultValue( false )]
		public bool NotifyOnValidationError
		{
			get { return binding.NotifyOnValidationError; }
			set { binding.NotifyOnValidationError = value; }
		}

		[DefaultValue( null )]
		public PropertyPath Path
		{
			get { return binding.Path; }
			set { binding.Path = value; }
		}

		[DefaultValue( null )]
		public RelativeSource RelativeSource
		{
			get { return binding.RelativeSource; }
			set { binding.RelativeSource = value; }
		}

		[DefaultValue( null )]
		public object Source
		{
			get { return binding.Source; }
			set { binding.Source = value; }
		}

		[DefaultValue( null )]
		public object TargetNullValue
		{
			get { return binding.TargetNullValue; }
			set { binding.TargetNullValue = value; }
		}

#if !SILVERLIGHT
		[DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden )]
		public UpdateSourceExceptionFilterCallback UpdateSourceExceptionFilter
		{
			get { return binding.UpdateSourceExceptionFilter; }
			set { binding.UpdateSourceExceptionFilter = value; }
		}
#endif

		[DefaultValue( UpdateSourceTrigger.Default )]
		public UpdateSourceTrigger UpdateSourceTrigger
		{
			get { return binding.UpdateSourceTrigger; }
			set { binding.UpdateSourceTrigger = value; }
		}

		[DefaultValue( false )]
		public bool ValidatesOnDataErrors
		{
			get { return binding.ValidatesOnDataErrors; }
			set { binding.ValidatesOnDataErrors = value; }
		}

		[DefaultValue( false )]
		public bool ValidatesOnExceptions
		{
			get { return binding.ValidatesOnExceptions; }
			set { binding.ValidatesOnExceptions = value; }
		}

#if !SILVERLIGHT
		[DefaultValue( null )]
		public string XPath
		{
			get { return binding.XPath; }
			set { binding.XPath = value; }
		}

		[DefaultValue( null )]
		public Collection<ValidationRule> ValidationRules
		{
			get { return binding.ValidationRules; }
		}
#endif

		/// <summary>
		/// This basic implementation just sets a binding on the targeted
		/// <see cref="DependencyObject"/> and returns the appropriate
		/// <see cref="BindingExpressionBase"/> instance.<br/>
		/// All this work is delegated to the decorated <see cref="Binding"/>
		/// instance.
		/// </summary>
		/// <returns>
		/// The object value to set on the property where the extension is applied. 
		/// In case of a valid binding expression, this is a <see cref="BindingExpressionBase"/>
		/// instance.
		/// </returns>
		/// <param name="provider">Object that can provide services for the markup
		/// extension.</param>
		public override object ProvideValue( IServiceProvider provider )
		{
			return binding.ProvideValue( provider );
		}

		/// <summary>
		/// Validates a service provider that was submitted to the <see cref="ProvideValue"/>
		/// method. This method checks whether the provider is null (happens at design time),
		/// whether it provides an <see cref="IProvideValueTarget"/> service, and whether
		/// the service's <see cref="IProvideValueTarget.TargetObject"/> and
		/// <see cref="IProvideValueTarget.TargetProperty"/> properties are valid
		/// <see cref="DependencyObject"/> and <see cref="DependencyProperty"/>
		/// instances.
		/// </summary>
		/// <param name="provider">The provider to be validated.</param>
		/// <param name="target">The binding target of the binding.</param>
		/// <param name="dp">The target property of the binding.</param>
		/// <returns>True if the provider supports all that's needed.</returns>
		protected bool TryGetTargetItems( IServiceProvider provider, out DependencyObject target, out DependencyProperty dp )
		{
			return this.TryGetTargetItems<DependencyObject>( provider, out target, out dp );
		}

		protected virtual bool TryGetTargetItems<T>( IServiceProvider provider, out T target, out DependencyProperty dp )
			where T : DependencyObject
		{
			target = null;
			dp = null;
			if( provider == null ) return false;

			//create a binding and assign it to the target
			var service = provider.GetService( typeof( IProvideValueTarget ) ) as IProvideValueTarget;
			if( service == null ) return false;

			//we need dependency objects / properties
			target = service.TargetObject as T;
			dp = service.TargetProperty as DependencyProperty;
			return target != null && dp != null;
		}

		/// <summary>
		/// Determines whether this binding is using a shared dependency property.
		/// </summary>
		/// <param name="provider">The provider.</param>
		/// <returns>
		/// 	<c>true</c> if is using a shared dependency property; otherwise, <c>false</c>.
		/// </returns>
		protected Boolean IsUsingSharedDependencyProperty( IServiceProvider provider )
		{
			var service = provider.GetService( typeof( IProvideValueTarget ) ) as IProvideValueTarget;
			if( service == null ) return false;

			return service.TargetObject != null && service.TargetObject.GetType().FullName == "System.Windows.SharedDp";
		}
	}
}