using System;
using System.Linq.Expressions;
using Topics.Radical.Linq;
using Topics.Radical.Validation;
using System.Globalization;

namespace Topics.Radical.Design
{
	interface IPropertyBuilder
	{
		DesignTimeProperty GetProperty();
	}

	/// <summary>
	/// A builder for a read-only design time property.
	/// </summary>
	/// <typeparam name="THost">The type of the host.</typeparam>
	/// <typeparam name="TProperty">The type of the property.</typeparam>
	public interface IReadOnlyPropertyBuilder<THost, TProperty> 
	{
		/// <summary>
		/// Adds a static value to the exposed property.
		/// </summary>
		/// <param name="value">The value.</param>
		void WithStaticValue( TProperty value );

        /// <summary>
        /// Adds a dynamic value to the exposed property, the given handler is 
        /// evaluated all the time the property is read by the designer.
        /// </summary>
        /// <param name="valueHandler">The value handler.</param>
        void WithDynamicValue( Func<CultureInfo, TProperty> valueHandler );

		/// <summary>
		/// Adds a live value to the exposed property, basically a live
		/// value property is able to connect a real property exposed by
		/// the design time host with a fake design time property; this
		/// behavior allows designers to lively manipulate design time
		/// properties value at design time directly from the xaml markup.
		/// </summary>
		/// <param name="liveValueProperty">The live value property.</param>
		void WithLiveValue( Expression<Func<TProperty>> liveValueProperty );

		/// <summary>
		/// Allows the developer to set any arbitrary value to the built
		/// property, it is quite obvious that the set value should be
		/// structurally similar to the exposed one. A typical use case
		/// is when the concrete view model expose a list of another view
		/// model but at design time the developer need to expose a list
		/// composed by design time instance ot the other vie model.
		/// </summary>
		/// <typeparam name="TObject">The type of the object.</typeparam>
		/// <param name="value">The value.</param>
		void WithSimilarValue<TObject>( TObject value );

        /// <summary>
        /// Adds a live value to the exposed property, basically a live
        /// value property is able to connect a real property exposed by
        /// the design time host with a fake design time property; this
        /// behavior allows designers to lively manipulate design time
        /// properties value at design time directly from the xaml markup.
        /// </summary>
        /// <param name="liveValueProperty">The live value property.</param>
        void WithLiveSimilarValue<TObject>( Expression<Func<TObject>> liveValueProperty );
	}

	/// <summary>
	/// A builder for a design time property.
	/// </summary>
	/// <typeparam name="THost">The type of the host.</typeparam>
	/// <typeparam name="TProperty">The type of the property.</typeparam>
	public interface IPropertyBuilder<THost, TProperty>
	{

        /// <summary>
        /// Marks the property as a property that holds localizable resources.
        /// </summary>
        /// <returns>The property builder instance.</returns>
        IPropertyBuilder<THost, TProperty> AsLocalizableResource();

		/// <summary>
		/// Marks the property as read only.
		/// </summary>
		/// <returns>The property builder instance.</returns>
		IReadOnlyPropertyBuilder<THost, TProperty> AsReadOnly();

		/// <summary>
		/// Adds a static value to the exposed property.
		/// </summary>
		/// <param name="value">The value.</param>
		void WithStaticValue( TProperty value );

        /// <summary>
        /// Adds a dynamic value to the exposed property, the given handler is 
        /// evaluated all the time the property is read by the designer.
        /// </summary>
        /// <param name="valueHandler">The value handler.</param>
        void WithDynamicValue( Func<CultureInfo, TProperty> valueHandler );

		/// <summary>
		/// Adds a live value to the exposed property, basically a live
		/// value property is able to connect a real property exposed by
		/// the design time host with a fake design time property; this
		/// behavior allows designers to lively manipulate design time
		/// properties value at design time directly from the xaml markup.
		/// </summary>
		/// <param name="liveValueProperty">The live value property.</param>
		void WithLiveValue( Expression<Func<TProperty>> liveValueProperty );

		/// <summary>
		/// Allows the developer to set any arbitrary value to the built
		/// property, it is quite obvious that the set value should be
		/// structurally similar to the exposed one. A typical use case
		/// is when the concrete view model expose a list of another view
		/// model but at design time the developer need to expose a list
		/// composed by design time instance ot the other vie model.
		/// </summary>
		/// <typeparam name="TObject">The type of the object.</typeparam>
		/// <param name="value">The value.</param>
		void WithSimilarValue<TObject>( TObject value );

        /// <summary>
        /// Adds a live value to the exposed property, basically a live
        /// value property is able to connect a real property exposed by
        /// the design time host with a fake design time property; this
        /// behavior allows designers to lively manipulate design time
        /// properties value at design time directly from the xaml markup.
        /// </summary>
        /// <param name="liveValueProperty">The live value property.</param>
        void WithLiveSimilarValue<TObject>( Expression<Func<TObject>> liveValueProperty );
	}
}
