using System;
using System.ComponentModel;
using System.Windows;
using Topics.Radical.Conversions;
using Topics.Radical.Windows.Presentation.ComponentModel;

namespace Topics.Radical.Windows.Presentation.Regions
{
	/// <summary>
	/// A base abstract implementation of the <see cref="IContentRegion"/>.
	/// </summary>
	/// <typeparam name="T">The type of the element that hosts this region.</typeparam>
	public abstract class ContentRegion<T> :
		Region<T>,
		IContentRegion
		where T : FrameworkElement
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ContentRegion&lt;T&gt;"/> class.
		/// </summary>
		protected ContentRegion()
		{

		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ContentRegion&lt;T&gt;"/> class.
		/// </summary>
		/// <param name="name">The name.</param>
		protected ContentRegion( String name )
			: base( name )
		{

		}

		/// <summary>
		/// Closes this instance.
		/// </summary>
		public override void Shutdown()
		{
			this.NotifyClosedAndEnsureRelease( this.Content );
		}

		/// <summary>
		/// Gets or sets the content.
		/// </summary>
		/// <value>
		/// The content.
		/// </value>
		public DependencyObject Content
		{
			get { return this.OnGetContent(); }
			set
			{
				if ( value != this.Content )
				{
					var args = new CancelEventArgs()
					{
						Cancel = false
					};

					var previous = this.Content;
					this.OnSetContent( value, args );
					if ( !args.Cancel )
					{
						this.OnContentSet( value, previous );
					}
				}
			}
		}

		/// <summary>
		/// Called to get content.
		/// </summary>
		/// <returns></returns>
		protected abstract DependencyObject OnGetContent();

		/// <summary>
		/// Called to set new content.
		/// </summary>
		/// <param name="view">The view.</param>
		/// <param name="args">The cancel even arguments.</param>
		protected abstract void OnSetContent( DependencyObject view, CancelEventArgs args );

		/// <summary>
		/// Called when content has been set.
		/// </summary>
		/// <param name="actual">The actual.</param>
		/// <param name="previous">The previous.</param>
		protected virtual void OnContentSet( DependencyObject actual, DependencyObject previous )
		{
			this.NotifyClosedAndEnsureRelease( previous );
		}
	}
}
