//using System.Collections;
//using System.Collections.Generic;
//using System.Linq;
//using Castle.Core;
//using Castle.Facilities;
//using Castle.MicroKernel.Facilities;
//using Topics.Radical.Reflection;
//using Topics.Radical.Windows.Presentation.ComponentModel.Regions;
//using Topics.Radical.Windows.Presentation.ComponentModel;
//using System;
//using Topics.Radical.Windows.Presentation.Boot;

//namespace Castle.Facilities
//{
//	public class InjectViewInRegionFacility : AbstractFacility
//	{
//		Dictionary<String, List<Type>> buffer = new Dictionary<string, List<Type>>();

//		void OnComponentRegistered( string key, MicroKernel.IHandler handler )
//		{
//			var model = handler.ComponentModel;
//			var conventions = this.Kernel.Resolve<BootstrapConventions>();

//			if( model.Service.Is<IRegionInjectionHandler>() )
//			{
//				if( this.buffer.Any() )
//				{
//					var mh = this.Kernel.Resolve<IRegionInjectionHandler>();
//					foreach( var kvp in this.buffer )
//					{
//						mh.RegisterViewsAsInterestedIn(
//							regionName: kvp.Key,
//							views: kvp.Value.AsEnumerable() );
//					}

//					this.buffer.Clear();
//				}
//			}
//			else
//			{
//				var regionName = conventions.GetInterestedRegionNameIfAny( model.Implementation );
//				if( !String.IsNullOrWhiteSpace( regionName ) )
//				{
//					var viewType = model.Implementation;

//					if( this.Kernel.HasComponent( typeof( IRegionInjectionHandler ) ) )
//					{
//						var mh = this.Kernel.Resolve<IRegionInjectionHandler>();
//						mh.RegisterViewAsInterestedIn( regionName, viewType );
//					}
//					else if( this.buffer.ContainsKey( regionName ) )
//					{
//						this.buffer[ regionName ].Add( viewType );
//					}
//					else
//					{
//						this.buffer.Add( regionName, new List<Type>() { viewType } );
//					}
//				}
//			}
//		}

//		/// <summary>
//		/// Performs the tasks associated with freeing, releasing, or resetting
//		/// the facility resources.
//		/// </summary>
//		protected override void Dispose()
//		{
//			base.Dispose();

//			this.Kernel.ComponentRegistered -= OnComponentRegistered;
//		}

//		/// <summary>
//		/// The custom initialization for the Facility.
//		/// </summary>
//		protected override void Init()
//		{
//			this.Kernel.ComponentRegistered += OnComponentRegistered;
//		}
//	}
//}
