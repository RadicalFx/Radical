using System;
using System.Diagnostics;
using System.Windows;
using Topics.Radical.ComponentModel.Windows.Input;
using Topics.Radical.Windows.Behaviors;
using Topics.Radical.Windows.Input;
using Topics.Radical.Windows.CommandBuilders;

namespace Topics.Radical.Windows.Markup
{
	public class AutoCommandBinding : CommandBinding
	{
		readonly static TraceSource logger = new TraceSource( typeof( AutoCommandBinding ).FullName );

		/// <summary>
		/// Initializes a new instance of the <see cref="AutoCommandBinding" /> class.
		/// </summary>
		public AutoCommandBinding()
		{

		}

		/// <summary>
		/// Initializes a new instance of the <see cref="AutoCommandBinding" /> class.
		/// </summary>
		/// <param name="path">The path.</param>
		public AutoCommandBinding( String path )
			: base( path )
		{

		}

		/// <summary>
		/// This basic implementation just sets a binding on the targeted
		/// <see cref="DependencyObject"/> and returns the appropriate
		/// <see cref="BindingExpressionBase"/> instance.<br/>
		/// All this work is delegated to the decorated <see cref="Binding"/>
		/// instance.
		/// </summary>
		/// <param name="provider">Object that can provide services for the markup
		/// extension.</param>
		/// <returns>
		/// The object value to set on the property where the extension is applied.
		/// In case of a valid binding expression, this is a <see cref="BindingExpressionBase"/>
		/// instance.
		/// </returns>
		public override object ProvideValue( IServiceProvider provider )
		{
			/*
			 * We basically have a problem here, since we cannot
			 * create the "runtime" delagate command until the target
			 * object is loaded if we return the base provided value,
			 * that is a "Binding" object the binding is runtime evaluated
			 * and null is returned because obviously the Path does not exists
			 * on the source object.
			 * 
			 * So if the base CommandBinding returns itself it means the we are
			 * currently using a Shared Dependency Property so we need to delay the 
			 * resolution; otherwise we return a fake empty command just to let the 
			 * Wpf binding engine to belive everything is working fine and wait for
			 * the target to be loaded.
			 */
			//var value = base.ProvideValue( provider );
			//if( Object.Equals( value, this ) )
			//{
			//    return this;
			//}

			base.ProvideValue( provider );

			if ( this.IsUsingSharedDependencyProperty( provider ) )
			{
				return this;
			}

			var fakePlaceholder = DelegateCommand.Create();

			return fakePlaceholder;
		}

		protected override void OnDataContextChanged( DependencyObject obj, DependencyProperty targetProperty, Object newValue, Object oldValue )
		{
			base.OnDataContextChanged( obj, targetProperty, newValue, oldValue );

			var actualCommand = obj.GetValue( targetProperty ) as DelegateCommand;
			if ( actualCommand != null )
			{
				var actualCommandData = actualCommand.GetData<CommandData>();
				var actualMonitor = actualCommandData != null ? actualCommandData.Monitor : null;

				if ( actualMonitor != null )
				{
					actualCommand.RemoveMonitor( actualMonitor );
					actualCommand.EvaluateCanExecute();
				}
			}

			if ( !DesignTimeHelper.GetIsInDesignMode() && newValue != null )
			{
				var newCommand = this.GetCommand( obj, targetProperty );
			}
		}

		protected virtual DelegateCommandBuilder GetCommandBuilder()
		{
			return new DelegateCommandBuilder();
		}

		protected override IDelegateCommand GetCommand( DependencyObject target, DependencyProperty targetProperty )
		{
			var builder = this.GetCommandBuilder();

			CommandData commandData;
			var dataContext = this.Source ?? builder.GetDataContext( target );

			if ( builder.CanCreateCommand( this.Path, target ) && builder.TryGenerateCommandData( this.Path, dataContext, out commandData ) )
			{
				var command = builder.CreateCommand( commandData );
				target.SetValue( targetProperty, command );

				return command;
			}

			//if( this.CanCreateCommand( target ) && this.TryGenerateCommandData( target, out commandData ) )
			//{
			//    var command = this.CreateCommand( target, commandData );
			//    target.SetValue( targetProperty, command );

			//    return command;
			//}

			return null;
		}
	}
}