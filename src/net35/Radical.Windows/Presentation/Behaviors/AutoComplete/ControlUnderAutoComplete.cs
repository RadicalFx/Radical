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
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Topics.Radical.Windows.Behaviors
{
	internal abstract class ControlUnderAutoComplete
	{
		private readonly Control control;

		internal static ControlUnderAutoComplete Create( Control control )
		{
			if( control is ComboBox )
				return new ComboBoxUnderAutoComplete( control );
			if( control is TextBox )
				return new TextBoxUnderAutoComplete( control );
			return null;
		}

		protected ControlUnderAutoComplete( Control control )
		{
			this.control = control;
		}

		public abstract DependencyProperty TextDependencyProperty { get; }

		public string Text
		{
			get { return ( string )control.GetValue( TextDependencyProperty ); }
			set { control.SetValue( TextDependencyProperty, value ); }
		}

		public Control Control
		{
			get { return control; }
		}

		public abstract string StyleKey { get; }

		public abstract void SelectAll();

		public abstract void Select( int start, int lenght );

		public abstract CollectionViewSource GetViewSource( Style style );

	}


	internal class TextBoxUnderAutoComplete : ControlUnderAutoComplete
	{

		public TextBoxUnderAutoComplete( Control control )
			: base( control )
		{
		}

		public override DependencyProperty TextDependencyProperty
		{
			get { return TextBox.TextProperty; }
		}

		public override string StyleKey
		{
			get { return "autoCompleteTextBoxStyle"; }
		}


		public override void SelectAll()
		{
			( ( TextBox )Control ).SelectAll();
		}

		public override void Select( int start, int lenght )
		{
			( ( TextBox )Control ).Select( start, lenght );
		}

		public override CollectionViewSource GetViewSource( Style style )
		{
			return ( CollectionViewSource )style.BasedOn.Resources[ "viewSource" ];
		}
	}

	internal class ComboBoxUnderAutoComplete : ControlUnderAutoComplete
	{

		public ComboBoxUnderAutoComplete( Control control )
			: base( control )
		{
		}

		public override DependencyProperty TextDependencyProperty
		{
			get { return ComboBox.TextProperty; }
		}

		public override string StyleKey
		{
			get { return "autoCompleteComboBoxStyle"; }
		}

		TextBox GetTextBox() 
		{
			var t = ( TextBox )this.Control.Template.FindName( "PART_EditableTextBox", this.Control );
			return t;
		}

		public override void SelectAll()
		{
			var t = this.GetTextBox();
			t.SelectAll();
		}

		public override void Select( int start, int lenght )
		{
			var t = this.GetTextBox();
			t.Select( start, lenght );
		}

		public override CollectionViewSource GetViewSource( Style style )
		{
			return ( CollectionViewSource )style.Resources[ "viewSource" ];
		}
	}
}