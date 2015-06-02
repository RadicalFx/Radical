using System;

namespace Castle
{
	/// <summary>
	/// Determines that a dependency is required.
	/// </summary>
	[AttributeUsage( 
		AttributeTargets.Property, 
		AllowMultiple = false, 
		Inherited = false )]
	public sealed class RequiredDependencyAttribute : Attribute
	{

	}
}
