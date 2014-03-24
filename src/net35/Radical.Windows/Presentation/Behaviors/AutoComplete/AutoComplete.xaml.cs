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
using System.ComponentModel;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using ik = System.Windows.Input;
using System.Windows.Media;

namespace Topics.Radical.Windows.Behaviors
{
	partial class AutoComplete
	{
		public interface ICanRepresentMyself
		{
			String AsString();
		}

		public interface IHaveAnOpinionOnFilter
		{
			Boolean Match( String userText );
		}

		private ControlUnderAutoComplete controlUnderAutocomplete;
		private CollectionViewSource viewSource;

		private bool iteratingListItems;
		private bool deletingText;
		private string rememberedText;
		private Popup autoCompletePopup;

		/// <summary>
		/// Initializes a new instance of the <see cref="AutoComplete"/> class.
		/// </summary>
		public AutoComplete( Control value )
		{
			InitializeComponent();

			this.controlUnderAutocomplete = ControlUnderAutoComplete.Create( value );

			this.viewSource = controlUnderAutocomplete.GetViewSource( ( Style )this[ this.controlUnderAutocomplete.StyleKey ] );
			this.viewSource.Filter += OnCollectionViewSourceFilter;

			this.controlUnderAutocomplete.Control.SetValue( Control.StyleProperty, this[ this.controlUnderAutocomplete.StyleKey ] );
			this.controlUnderAutocomplete.Control.ApplyTemplate();

			this.autoCompletePopup = ( Popup )this.controlUnderAutocomplete.Control.Template.FindName( "autoCompletePopup", this.controlUnderAutocomplete.Control );
			this._listBox = ( ListBox )this.controlUnderAutocomplete.Control.Template.FindName( "autoCompleteListBox", this.controlUnderAutocomplete.Control );

			var b = new Binding( "ActualWidth" )
			{
				UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
				Source = this.controlUnderAutocomplete.Control
			};

			this.ListBox.SetBinding( ListBox.MinWidthProperty, b );
			this.ListBox.PreviewMouseDown += OnListBoxPreviewMouseDown;

			this.controlUnderAutocomplete.Control.AddHandler( TextBox.TextChangedEvent, new TextChangedEventHandler( OnTextBoxTextChanged ) );
			this.controlUnderAutocomplete.Control.LostFocus += OnTextBoxLostFocus;
			this.controlUnderAutocomplete.Control.PreviewKeyUp += OnTextBoxPreviewKeyUp;
			this.controlUnderAutocomplete.Control.PreviewKeyDown += OnTextBoxPreviewKeyDown;
		}

		internal CollectionViewSource ViewSource
		{
			get { return viewSource; }
		}

		private ListBox _listBox;
		private ListBox ListBox
		{
			get { return this._listBox; }
		}

		private void OnCollectionViewSourceFilter( object sender, FilterEventArgs e )
		{
			if( AutoComplete.GetImplicitItemsFilter( controlUnderAutocomplete.Control ) == ImplicitItemsFilter.Enabled )
			{
				var iho = e.Item as AutoComplete.IHaveAnOpinionOnFilter;
				AutoCompleteFilterPathCollection filterPaths = GetAutoCompleteFilterProperty();
				if( iho != null )
				{
					e.Accepted = iho.Match( controlUnderAutocomplete.Text );
				}
				else if( filterPaths != null )
				{
					Type t = e.Item.GetType();
					foreach( string autoCompleteProperty in filterPaths )
					{
						PropertyInfo info = t.GetProperty( autoCompleteProperty );
						object value = info.GetValue( e.Item, null );
						if( TextBoxStartsWith( value ) )
						{
							e.Accepted = true;
							return;
						}
					}
					e.Accepted = false;
				}
				else
				{
					e.Accepted = TextBoxStartsWith( e.Item );
				}
			}
			else
			{
				e.Accepted = true;
			}
		}

		private bool TextBoxStartsWith( object value )
		{
			return value != null && value.ToString().StartsWith( controlUnderAutocomplete.Text, StringComparison.CurrentCultureIgnoreCase );
		}

		private AutoCompleteFilterPathCollection GetAutoCompleteFilterProperty()
		{
			if( GetFilterPath( controlUnderAutocomplete.Control ) != null )
				return GetFilterPath( controlUnderAutocomplete.Control );
			return null;
		}

		Boolean ignoreOnTextBoxTextChanged = false;

		private void OnTextBoxTextChanged( object sender, TextChangedEventArgs e )
		{
			if( !this.ignoreOnTextBoxTextChanged )
			{
				SetUserText( this.controlUnderAutocomplete.Control, this.controlUnderAutocomplete.Text );

				if( controlUnderAutocomplete.Text == "" )
				{
					autoCompletePopup.IsOpen = false;
					return;
				}
				if( !iteratingListItems && this.controlUnderAutocomplete.Text != "" )
				{
					ICollectionView v = viewSource.View;
					v.Refresh();
					if( v.IsEmpty )
					{
						autoCompletePopup.IsOpen = false;
					}
					else
					{
						autoCompletePopup.IsOpen = true;

						if( v.CurrentItem == null )
						{
							v.MoveCurrentToFirst();
						}

						if( !this.deletingText && v.CurrentItem != null )
						{
							var item = this.GetTextForTextBox( v.CurrentItem );

							//var userText = this.controlUnderAutocomplete.Text;
							//var diff = item.Remove( 0, userText.Length );
							//var firstAppendedCharIndex = item.IndexOf( diff );

							this.ignoreOnTextBoxTextChanged = true;
							this.controlUnderAutocomplete.Text = item;
							this.ignoreOnTextBoxTextChanged = false;

							//this.controlUnderAutocomplete.Select( firstAppendedCharIndex, item.Length );
						}
					}
				}
			}
		}

		private void OnTextBoxPreviewKeyDown( object sender, KeyEventArgs e )
		{
			this.deletingText = e.Key == ik.Key.Delete || e.Key == ik.Key.Back;

			//if( e.Key == ik.Key.Tab )
			//{
			//    this.OnItemChoosen();
			//}
		}

		private void OnTextBoxPreviewKeyUp( object sender, KeyEventArgs e )
		{
			if( e.Key == ik.Key.Up || e.Key == ik.Key.Down )
			{
				if( this.rememberedText == null )
				{
					this.rememberedText = controlUnderAutocomplete.Text;
				}

				this.iteratingListItems = true;
				var view = viewSource.View;

				if( e.Key == ik.Key.Up )
				{
					if( view.CurrentItem == null )
						view.MoveCurrentToLast();
					else
						view.MoveCurrentToPrevious();
				}
				else
				{
					if( view.CurrentItem == null )
						view.MoveCurrentToFirst();
					else
						view.MoveCurrentToNext();
				}

				if( view.CurrentItem == null )
				{
					this.controlUnderAutocomplete.Text = rememberedText;
				}
				else
				{
					this.controlUnderAutocomplete.Text = this.GetTextForTextBox( view.CurrentItem );
				}
			}
			else
			{
				this.iteratingListItems = false;
				this.rememberedText = null;
				if( this.autoCompletePopup.IsOpen && ( e.Key == ik.Key.Escape || e.Key == ik.Key.Enter ) )
				{
					this.autoCompletePopup.IsOpen = false;
					if( e.Key == ik.Key.Enter )
					{
						this.controlUnderAutocomplete.SelectAll();
						this.OnItemChoosen();
					}
				}
			}
		}

		private void OnItemChoosen()
		{ 
			var view = this.viewSource.View;
			var ci = view.CurrentItem;

			SetChoosenItem( this.controlUnderAutocomplete.Control, ci );
		}

		private void OnTextBoxLostFocus( object sender, RoutedEventArgs e )
		{
			this.autoCompletePopup.IsOpen = false;
			this.OnItemChoosen();
		}

		private void OnListBoxPreviewMouseDown( object sender, MouseButtonEventArgs e )
		{
			var pt = e.GetPosition( ( IInputElement )sender );
			var hr = VisualTreeHelper.HitTest( this.ListBox, pt );
			if( hr != null )
			{
				var lbi = hr.VisualHit.FindParent<ListBoxItem>();
				if( lbi != null && lbi.DataContext != null )
				{
					this.controlUnderAutocomplete.Text = this.GetTextForTextBox( lbi.DataContext );
					this.autoCompletePopup.IsOpen = false;
					this.controlUnderAutocomplete.SelectAll();
					this.OnItemChoosen();
				}
			}
		}

		String GetTextForTextBox( Object selectedItem ) 
		{
			var icrm = selectedItem as ICanRepresentMyself;
			var value = icrm != null ? icrm.AsString() : selectedItem.ToString();

			return value;
		}
	}
}
