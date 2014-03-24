namespace Topics.Radical.Model
{
	/// <summary>
	/// Identifies a strongly-typed property value.
	/// </summary>
	/// <typeparam name="T">The type of the stored value.</typeparam>
	public class PropertyValue<T> : PropertyValue
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="PropertyValue&lt;T&gt;"/> class.
		/// </summary>
		/// <param name="value">The value.</param>
		public PropertyValue( T value )
		{
			this.Value = value;
		}

		/// <summary>
		/// Gets the stored property value.
		/// </summary>
		/// <returns>The stored value.</returns>
		public override object GetValue()
		{
			return this.Value;
		}

		/// <summary>
		/// Gets the stored value.
		/// </summary>
		/// <value>The stored value.</value>
		public T Value { get; private set; }
	}
}
