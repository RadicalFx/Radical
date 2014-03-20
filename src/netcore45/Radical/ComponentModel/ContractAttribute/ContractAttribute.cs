namespace Topics.Radical.ComponentModel
{
	using System;

	/// <summary>
	/// Represents a contract interface
	/// </summary>
	[AttributeUsage( AttributeTargets.Interface, AllowMultiple = false, Inherited = true )]
	public sealed class ContractAttribute : Attribute
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ContractAttribute"/> class.
		/// </summary>
		public ContractAttribute()
		{
			
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ContractAttribute"/> class.
		/// </summary>
		/// <param name="contractInterface">The contract interface.</param>
		public ContractAttribute( Type contractInterface )
		{
			if( contractInterface == null )
			{
				throw new ArgumentNullException( "contractInterface" );
			}

			this.ContractInterface = contractInterface;
		}

		/// <summary>
		/// Gets the contract interface.
		/// </summary>
		/// <value>The contract interface.</value>
		public Type ContractInterface
		{
			get;
			private set;
		}
	}
}
