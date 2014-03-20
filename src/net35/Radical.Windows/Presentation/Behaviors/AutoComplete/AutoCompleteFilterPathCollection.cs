//Copyright (c) 2008 Jason Kemp
//
//Permission is hereby granted, free of charge, to any person obtaining a copy
//of this software and associated documentation files (the "Software"), to deal
//in the Software without restriction, including without limitation the rights
//to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//copies of the Software, and to permit persons to whom the Software is
//furnished to do so, subject to the following conditions:
//
//The above copyright notice and this permission notice shall be included in
//all copies or substantial portions of the Software.
//
//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
//THE SOFTWARE.
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Topics.Radical.Windows.Behaviors
{
	[TypeConverter( typeof( AutoCompleteFilterPathCollectionTypeConverter ) )]
	public class AutoCompleteFilterPathCollection : Collection<string>
	{
		public AutoCompleteFilterPathCollection( IList<string> list )
			: base( list )
		{
		}

		public AutoCompleteFilterPathCollection()
		{
		}

		internal string Join()
		{
			string[] array = new string[ this.Count ];
			this.CopyTo( array, 0 );
			return String.Join( ",", array );
		}
	}

	internal class AutoCompleteFilterPathCollectionTypeConverter : TypeConverter
	{
		public override bool CanConvertFrom( ITypeDescriptorContext context, Type sourceType )
		{
			if( sourceType == typeof( string ) )
				return true;
			return base.CanConvertFrom( context, sourceType );
		}

		public override bool CanConvertTo( ITypeDescriptorContext context, Type destinationType )
		{
			if( destinationType == typeof( string ) )
				return true;
			return base.CanConvertTo( context, destinationType );
		}

		public override object ConvertFrom( ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value )
		{
			if( value is string )
			{
				string[] paths = ( ( string )value ).Split( new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries );
				return new AutoCompleteFilterPathCollection( paths );
			}
			return base.ConvertFrom( context, culture, value );
		}

		public override object ConvertTo( ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType )
		{
			if( destinationType == typeof( string ) )
			{
				AutoCompleteFilterPathCollection c = ( AutoCompleteFilterPathCollection )value;
				return c.Join();
			}
			return base.ConvertTo( context, culture, value, destinationType );
		}
	}
}