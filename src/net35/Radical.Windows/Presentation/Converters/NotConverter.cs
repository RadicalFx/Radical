using System;
using System.Windows.Data;
using Topics.Radical.Validation;
using System.Windows.Markup;

namespace Topics.Radical.Windows.Converters
{
	[MarkupExtensionReturnType( typeof( NotConverter ) )]
	[ValueConversion( typeof( Boolean ), typeof( Boolean ) )]
	public sealed class NotConverter : AbstractSingletonConverter
	{
        //static WeakReference singleton = new WeakReference( null );

        //public override object ProvideValue( IServiceProvider serviceProvider )
        //{
        //    IValueConverter converter = ( IValueConverter )singleton.Target;
        //    if( converter == null )
        //    {
        //        converter = this;
        //        singleton.Target = converter;
        //    }

        //    return converter;
        //}

		public override object Convert( object value, Type targetType, object parameter, System.Globalization.CultureInfo culture )
		{
			Ensure.That( value ).Named( "value" )
				.IsNotNull()
				.IsTrue( v => v is Boolean );

			Ensure.That( targetType ).Is<Boolean>();

			return !( ( Boolean )value );
		}

		public override object ConvertBack( object value, Type targetType, object parameter, System.Globalization.CultureInfo culture )
		{
			Ensure.That( value ).Named( "value" ).IsNotNull();
			Ensure.That( targetType ).Is( typeof( Boolean ) );

			return !( ( Boolean )value );
		}
	}
}