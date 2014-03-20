using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.IO;
using System.Windows.Media.Imaging;
//using log4net;
using Topics.Radical.Diagnostics;
using System.Windows.Media;
using System.Windows.Markup;
using System.Diagnostics;

namespace Topics.Radical.Windows.Converters
{
	[MarkupExtensionReturnType( typeof( BinaryImageConverter ) )]
	public class BinaryImageConverter : AbstractSingletonConverter
	{
		static readonly TraceSource logger = new TraceSource( typeof( BinaryImageConverter ).FullName );
		//static readonly ILog logger = LogManager.GetLogger( typeof( BinaryImageConverter ) );

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
		public override Object Convert( object value, Type targetType, object parameter, System.Globalization.CultureInfo culture )
		{
			var valueIsValid = value is Byte[];
			var targetTypeIsValid = targetType == typeof( ImageSource );

			if( value == null )
				logger.Warning( "BinaryImageConverter.Convert(): source value is null." );

			if( !valueIsValid )
				logger.Warning( "BinaryImageConverter.Convert(): source value is not a Byte[]." );

			if( !targetTypeIsValid )
				logger.Warning( "BinaryImageConverter.Convert(): TargetType is not BitmapImage." );

			if( targetTypeIsValid && valueIsValid )
			{
				var stream = new MemoryStream( ( Byte[] )value );
				var image = new BitmapImage();
				image.BeginInit();
				image.StreamSource = stream;
				image.EndInit();

				return image;
			}

			return null;
		}

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
		public override Object ConvertBack( object value, Type targetType, object parameter, System.Globalization.CultureInfo culture )
		{
			//var valueIsValid = value is BitmapImage;
			//var targetTypeIsValid = targetType == typeof( Byte[] );

			//logger.WarnIf( value == null, "BinaryImageConverter.ConvertBack(): source value is null." );
			//logger.WarnIf( !valueIsValid, "BinaryImageConverter.ConvertBack(): source value is not a BitmapImage." );
			//logger.WarnIf( !targetTypeIsValid, "BinaryImageConverter.ConvertBack(): TargetType is not Byte[]." );

			//if( targetTypeIsValid && valueIsValid )
			//{
			//    var image = ( BitmapImage )value;

			//    using( var ms = new MemoryStream() )
			//    {
			//        var encoder = new BmpBitmapEncoder();
			//        encoder.Frames.Add( BitmapFrame.Create( image ) );
			//        encoder.Save( ms );

			//        ms.Position = 0;
			//        return ms.GetBuffer();
			//    }
			//}

			//return null;

			throw new NotSupportedException();
		}
	}
}
