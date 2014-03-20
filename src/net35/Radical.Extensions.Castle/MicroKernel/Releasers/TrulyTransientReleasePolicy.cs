using System;
using Castle.Core;
using Castle.MicroKernel.Lifestyle;

namespace Castle.MicroKernel.Releasers
{
	/// <summary>
	/// Inherits from the default ReleasePolicy; do not track our own transient objects.
	/// Only tracks components that have decommission steps
	/// registered or have pooled lifestyle.
	/// </summary>
	[Serializable]
	public class TrulyTransientReleasePolicy : LifecycledComponentsReleasePolicy
	{
		/// <summary>
		/// Gets policy the behavior.
		/// </summary>
		/// <value>The behavior.</value>
		public TrulyTransientReleasePolicyBehavior Behavior
		{
			get;
			private set;
		}

        /// <summary>
        /// Initializes a new instance of the <see cref="TrulyTransientReleasePolicy" /> class.
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        /// <param name="behavior">The behavior.</param>
		public TrulyTransientReleasePolicy( IKernel kernel, TrulyTransientReleasePolicyBehavior behavior )
            : base( kernel )
		{
			this.Behavior = behavior;
		}

		/// <summary>
		/// Tracks the specified instance.
		/// </summary>
		/// <param name="instance">The instance.</param>
		/// <param name="burden">The burden.</param>
		public override void Track( object instance, Burden burden )
		{
			var model = burden.Model;

			if( this.Behavior == TrulyTransientReleasePolicyBehavior.IgnoreAllTransientObjects && model.LifestyleType == LifestyleType.Transient )
			{
				return;
			}

			// we skip the tracking for object marked with our custom Transient lifestyle manager      
			if( ( model.LifestyleType == LifestyleType.Custom ) && ( typeof( TrulyTransientLifestyleManager ) == model.CustomLifestyle ) )
			{
				return;
			}

			base.Track( instance, burden );
		}
	}
}
