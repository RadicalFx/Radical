using System;
using System.Windows.Data;
using Topics.Radical.Conversions;
using System.Reflection;
using Topics.Radical.Validation;
using System.Windows.Markup;
using Topics.Radical.Windows.Behaviors;
using Topics.Radical.Reflection;

namespace Topics.Radical.Windows.Converters
{
	[MarkupExtensionReturnType( typeof( EnumCaptionConverter ) )]
	public sealed class EnumCaptionConverter : AbstractSingletonConverter
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
			if( value != null && targetType == typeof( string ) )
			{
				Ensure.That( value )
					.WithMessage( "Supplied value is not an Enum value." )
					.IsTrue( v => v.GetType().Is<Enum>() )
					.WithMessage( "Supplied enum value does not define the required EnumItemDescriptionAttribute." )
					.IsTrue( v => ( ( Enum )v ).IsDescriptionAttributeDefined() );

				return ( ( Enum )value ).GetCaption();
			}

			return null;
		}

		public override object ConvertBack( object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture )
		{
			throw new System.NotImplementedException();
		}
	}
}