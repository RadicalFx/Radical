using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Markup;
using System.Windows.Data;

namespace Topics.Radical.Windows.Converters
{
	public abstract class AbstractSingletonConverter : MarkupExtension, IValueConverter
	{
        static Dictionary<Type, WeakReference> singletons = new Dictionary<Type,WeakReference>();

        /// <summary>
        /// When implemented in a derived class, returns an object that is set as the value of the target property for this markup extension.
        /// </summary>
        /// <param name="serviceProvider">Object that can provide services for the markup extension.</param>
        /// <returns>
        /// The object value to set on the property where the extension is applied.
        /// </returns>
        public override sealed object ProvideValue( IServiceProvider serviceProvider )
        {
            var type = this.GetType();
            if ( !singletons.ContainsKey( type ) ) 
            {
                singletons.Add( type, new WeakReference( this ) );
            }

            var wr = singletons[type];
            IValueConverter converter = ( IValueConverter )wr.Target;
            if( converter == null )
            {
                converter = this;
                wr.Target = converter;
            }

            return converter;
        }

		/// <summary>
		/// Converts a value.
		/// </summary>
		/// <param name="value">The value produced by the binding source.</param>
		/// <param name="targetType">The type of the binding target property.</param>
		/// <param name="parameter">The converter parameter to use.</param>
		/// <param name="culture">The culture to use in the converter.</param>
		/// <returns>
		/// A converted value. If the method returns null, the valid null value is used.
		/// </returns>
		public abstract object Convert( object value, Type targetType, object parameter, System.Globalization.CultureInfo culture );

		/// <summary>
		/// Converts a value.
		/// </summary>
		/// <param name="value">The value that is produced by the binding target.</param>
		/// <param name="targetType">The type to convert to.</param>
		/// <param name="parameter">The converter parameter to use.</param>
		/// <param name="culture">The culture to use in the converter.</param>
		/// <returns>
		/// A converted value. If the method returns null, the valid null value is used.
		/// </returns>
		public abstract object ConvertBack( object value, Type targetType, object parameter, System.Globalization.CultureInfo culture );
	}
}
