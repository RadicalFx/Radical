using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Markup;
using System.Windows.Media;
using Topics.Radical.Helpers;
using Topics.Radical.Reflection;
using Topics.Radical.Windows.Presentation.ComponentModel;
using Topics.Radical.Conversions;

#if !SILVERLIGHT
using Topics.Radical.Diagnostics;
#endif

namespace Topics.Radical.Windows.Presentation.Regions
{
	/// <summary>
	/// A base abstract implementation of a region and it's relative markup extension.
	/// </summary>
	/// <typeparam name="T">The type of the element that hosts this region.</typeparam>
#if !SILVERLIGHT
	[MarkupExtensionReturnType( typeof( IRegion ) )]
#endif
	public abstract class Region<T> :
		MarkupExtension,
		IRegion where T : FrameworkElement
	{
#if !SILVERLIGHT
		/// <summary>
		/// The logger.
		/// </summary>
		protected readonly TraceSource Logger;
#endif

#if SILVERLIGHT
		/// <summary>
		/// Occurs when this region is ready.
		/// </summary>
		public event EventHandler Ready = ( s, e ) => { };
#endif

		/// <summary>
		/// Initializes a new instance of the <see cref="Region&lt;T&gt;"/> class.
		/// </summary>
		protected Region()
		{
#if !SILVERLIGHT
			Logger = new TraceSource( this.GetType().Name );
#endif
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Region&lt;T&gt;"/> class.
		/// </summary>
		/// <param name="name">The name.</param>
		protected Region( String name )
			: this()
		{
			this.Name = name;
		}

		/// <summary>
		/// Gets the view that hosts this region.
		/// </summary>
		public DependencyObject HostingView { get; private set; }

		/// <summary>
		/// Gets the name of the region.
		/// </summary>
		/// <value>
		/// The name.
		/// </value>
		public String Name { get; set; }

		/// <summary>
		/// The number of milliseconds to wait before loading the region content.
		/// </summary>
		public Int32 AsyncLoadDelay { get; set; }

		/// <summary>
		/// Gets the element.
		/// </summary>
		protected T Element
		{
			get;
			private set;
		}

		/// <summary>
		/// Called when when the element is changed.
		/// </summary>
		protected virtual void OnElementChanged()
		{

		}

		/// <summary>
		/// When implemented in a derived class, returns an object that is set as the value of the target property for this markup extension.
		/// </summary>
		/// <param name="serviceProvider">Object that can provide services for the markup extension.</param>
		/// <returns>
		/// The object value to set on the property where the extension is applied.
		/// </returns>
		public override sealed object ProvideValue( IServiceProvider serviceProvider )
		{
			if ( serviceProvider != null )
			{
				var service = serviceProvider.GetService( typeof( IProvideValueTarget ) ) as IProvideValueTarget;
				if ( service != null )
				{
					var element = service.TargetObject as T;
					if ( element == null )
					{
						//TODO: migliorare l'exception
						var msg = String.Format( "The TargetObject of this {0} is null.", this.GetType().ToString( "SN" ) );
						throw new NotSupportedException( msg );
					}

					if ( this.Element != element )
					{
						this.Element = element;
						this.OnElementChanged();
					}

					if ( !DesignerProperties.GetIsInDesignMode( this.Element ) )
					{
#if SILVERLIGHT

						RoutedEventHandler loaded = null;

						loaded = (s,e)=>
						{
						    var view = this.FindHostingViewOf( this.Element );

						    if( view == null )
						    {
						        //TODO: migliorare l'exception
						        var msg = String.Format( "Cannot find any hosting view for this {0}.",
						            this.GetType().ToString( "SN" ) );

						        throw new NotSupportedException( msg );
						    }

						    this.HostingView = view;

							var h = this.Ready;
							h( this, EventArgs.Empty );

							this.Element.Loaded -= loaded;
						};

						this.Element.Loaded += loaded;
#else

						var view = this.FindHostingViewOf( this.Element );

						if ( view == null )
						{
							//TODO: migliorare l'exception
							var msg = String.Format( "Cannot find any hosting view for this {0}.",
								this.GetType().ToString( "SN" ) );

							throw new NotSupportedException( msg );
						}

						this.HostingView = view;
#endif

#if !SILVERLIGHT

						if ( CommandLine.GetCurrent().Contains( "hr" ) )
						{
							this.Logger.Warning( "Regions hilighting is turned on." );

							this.Element.Loaded += ( s, e ) =>
							{
								var obj = ( UIElement )s;
								var layer = AdornerLayer.GetAdornerLayer( obj );
								Debug.WriteLineIf( layer == null, "Region: cannot find any AdornerLayer on the given element." );
								if ( layer != null )
								{
									var adorner = new RegionHilightAdorner( obj, this, Brushes.Red );
									layer.Add( adorner );
								}
							};

							this.Element.Unloaded += ( s, e ) =>
							{
								var obj = ( UIElement )s;
								var layer = AdornerLayer.GetAdornerLayer( obj );
								Debug.WriteLineIf( layer == null, "Region: cannot find any AdornerLayer on the given element." );

								if ( layer != null )
								{
									var adorners = layer.GetAdorners( obj );
									Debug.WriteLineIf( adorners == null, "Region: cannot find any Adorner on the given element." );

									if ( adorners != null )
									{
										var la = adorners.Where( a => a is RegionHilightAdorner ).SingleOrDefault();
										if ( la != null )
										{
											layer.Remove( la );
										}
									}
								}
							};
						}

#endif
					}
				}
			}

			return this;
		}

		/// <summary>
		/// Closes this region, the close process is invoked by the host at close time.
		/// </summary>
		public abstract void Shutdown();

		/// <summary>
		/// Notify via IExpectViewClosedCallback and release the resources, even for nested views.
		/// </summary>
		/// <param name="view"></param>
		protected virtual void NotifyClosedAndEnsureRelease( DependencyObject view )
		{
			if ( RegionService.Conventions == null )
			{
				//BUG: se è null l'app sta facendo shutdown.
				return;
			}

			if ( view != null )
			{
				if ( RegionService.Conventions.ShouldUnregisterRegionManagerOfView( view ) && RegionService.CurrentService.HoldsRegionManager( view ) )
				{
					RegionService.CurrentService.UnregisterRegionManager( view, UnregisterBehavior.WholeLogicalTreeChain );
				}

				RegionService.Conventions
					.GetViewDataContext( view, RegionService.Conventions.DefaultViewDataContextSearchBehavior )
					.As<IExpectViewClosedCallback>( i => i.OnViewClosed() );

				if ( RegionService.Conventions.ShouldReleaseView( view ) )
				{
					RegionService.Conventions.ViewReleaseHandler( view, ViewReleaseBehavior.Default );
				}
			}
		}

		/// <summary>
		/// Finds the hosting view of the given FramerowkElement.
		/// </summary>
		/// <param name="fe">The fe.</param>
		/// <returns></returns>
		protected virtual DependencyObject FindHostingViewOf( FrameworkElement fe )
		{
			if ( fe == null )
			{
				return null;
			}
			else if ( RegionService.Conventions.IsHostingView( fe ) )
			{
				return fe;
			}
			else if ( fe.Parent != null )
			{
				return FindHostingViewOf( fe.Parent as FrameworkElement );
			}

			return null;
		}
	}
}
