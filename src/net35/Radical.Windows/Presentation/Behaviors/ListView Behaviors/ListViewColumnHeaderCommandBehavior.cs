using System;
using System.Linq;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows;

namespace Topics.Radical.Windows.Behaviors
{
	public class ListViewColumnHeaderCommandBehavior :
		RadicalBehavior<ListView>,
		ICommandSource
	{
		readonly RoutedEventHandler onLoaded;
		readonly RoutedEventHandler onColumnHeaderClick;

		public ListViewColumnHeaderCommandBehavior()
		{
			onColumnHeaderClick = ( s, e ) =>
			{
				var clickedHeader = e.OriginalSource as GridViewColumnHeader;
				if( clickedHeader != null && clickedHeader.Role != GridViewColumnHeaderRole.Padding )
				{
					var column = clickedHeader.Column;
					String commandParam = null;

					if( column.DisplayMemberBinding is Binding )
					{
						commandParam = ( ( Binding )column.DisplayMemberBinding ).Path.Path;
					}
					else
					{
						commandParam = GridViewColumnManager.GetSortProperty( column );
					}

					if( !String.IsNullOrEmpty( commandParam ) && this.Command != null && this.Command.CanExecute( commandParam ) )
					{
						this.Command.Execute( commandParam );
					}
				}
			};

			onLoaded = ( s, e ) =>
			{
				this.AssociatedObject.AddHandler(
					GridViewColumnHeader.ClickEvent,
					onColumnHeaderClick );
			};
		}

		#region Dependency Property: Command

		public static readonly DependencyProperty CommandProperty = DependencyProperty.Register(
			"Command",
			typeof( ICommand ),
			typeof( ListViewColumnHeaderCommandBehavior ),
			new PropertyMetadata( null ) );

		public ICommand Command
		{
			get { return ( ICommand )this.GetValue( CommandProperty ); }
			set { this.SetValue( CommandProperty, value ); }
		}

		#endregion

		protected override void OnAttached()
		{
			base.OnAttached();

			this.AssociatedObject.Loaded += onLoaded;
		}

		protected override void OnDetaching()
		{
			base.OnDetaching();

			this.AssociatedObject.Loaded -= onLoaded;
			this.AssociatedObject.RemoveHandler(
					GridViewColumnHeader.ClickEvent,
					onColumnHeaderClick );
		}

		public object CommandParameter
		{
			get;
			set;
		}

		/// <summary>
		/// The object that the command is being executed on.
		/// </summary>
		/// <value></value>
		public IInputElement CommandTarget
		{
			get { return this.AssociatedObject as IInputElement; }
		}
	}
}
