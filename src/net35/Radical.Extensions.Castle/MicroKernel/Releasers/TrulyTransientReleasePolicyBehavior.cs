using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.MicroKernel.Lifestyle;
using Castle.MicroKernel.Releasers;
using Castle.Core;
using Castle.MicroKernel;

namespace Castle.MicroKernel.Releasers
{
	/// <summary>
	/// Defines the way the <see cref="TrulyTransientReleasePolicy"/> behaves.
	/// </summary>
	public enum TrulyTransientReleasePolicyBehavior
	{
		/// <summary>
		/// All transient objects are ignored and not tracked.
		/// </summary>
		IgnoreAllTransientObjects,

		/// <summary>
		/// Only transient objects registered with the
		/// <see cref="TrulyTransientLifestyleManager"/>
		/// are ignored and not tracked.
		/// </summary>
		IgnoreTransientObjectsWithCustomLifestyle
	}
}
