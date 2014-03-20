using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Topics.Radical.ComponentModel.Windows.Input;
using System.Reflection;
using Topics.Radical.Linq;
using Topics.Radical.Windows.Input;
using Topics.Radical.Windows.Behaviors;
using Topics.Radical.Reflection;

namespace Topics.Radical.Windows.Markup
{
	public class BehaviorCommandBinding : CommandBinding
	{
		protected override void OnProvideValue( IServiceProvider provider, object value )
		{
			DependencyObject fe;
			DependencyProperty dp;

			if( this.TryGetTargetItems( provider, out fe, out dp ) )
			{
				var inab = fe as INotifyAttachedOjectLoaded;
				if( inab != null )
				{
					EventHandler h = null;
					h = ( s, e ) =>
					{
						inab.AttachedObjectLoaded -= h;
						this.OnTargetLoaded( fe, dp );
					};

					inab.AttachedObjectLoaded += h;
				}
			}
		}

		protected override void SetInputBindings( DependencyObject target, ICommandSource source, IDelegateCommand command )
		{
			//Not supported ?
		}

		protected override IDelegateCommand GetCommand( DependencyObject target, DependencyProperty targetProperty )
		{
			if( this.Path != null && target is INotifyAttachedOjectLoaded )
			{
				var dataContext = ( ( INotifyAttachedOjectLoaded )target )
					.GetAttachedObject<FrameworkElement>()
					.DataContext;
				
				var path = this.Path.Path;
				var methodName = path.EndsWith( "Command" ) ? path.Remove( path.IndexOf( "Command" ) ) : path;
				var method = dataContext.GetType().GetMethod( methodName );

				var def = dataContext.GetType()
					.GetMethods()
					.Where( mi => mi.Name.Equals( methodName ) )
					.Select( mi =>
					{
						var prms = mi.GetParameters();

						return new
						{
							FastDelegate = mi.CreateVoidDelegate(),
							DataContext = dataContext,
							HasParameter = prms.Length == 1,
							ParameterType = prms.Length != 1 ? null : prms[ 0 ].ParameterType,
							KeyBindings = mi.GetAttributes<KeyBindingAttribute>(),
							Description = method.GetAttribute<CommandDescriptionAttribute>(),
							Fact = dataContext.GetType()
										.GetProperties()
										.Where( pi => pi.PropertyType == typeof( Fact ) && pi.Name.Equals( "Can" + methodName ) )
										.Select( pi => ( Fact )pi.GetValue( dataContext, null ) )
										.SingleOrDefault()
						};
					} )
					.SingleOrDefault();

				var text = ( def.Description == null ) ? String.Empty : def.Description.DisplayText;
				var cmd = DelegateCommand.Create( text )
					.OnCanExecute( o =>
					{
						return def.Fact != null ?
							def.Fact.Eval( o ) :
							true;
					} )
					.OnExecute( o =>
					{
						if( def.HasParameter )
						{
							var prm = Convert.ChangeType( o, def.ParameterType );
							def.FastDelegate( def.DataContext, new[] { prm } );
						}
						else
						{
							def.FastDelegate( def.DataContext, null );
						}
					} );

				if( def.KeyBindings != null )
				{
					def.KeyBindings
						.ForEach( kb => cmd.AddKeyGesture( kb.Key, kb.Modifiers ) );
				}

				if( def.Fact != null )
				{
					cmd.AddMonitor( def.Fact );
				}

				target.SetValue( targetProperty, cmd );

				return cmd;
			}

			return null;
		}
	}
}