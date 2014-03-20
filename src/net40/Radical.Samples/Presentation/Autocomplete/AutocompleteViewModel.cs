using System;
using System.Linq;
using Topics.Radical.ComponentModel;
using Topics.Radical.Observers;
using Topics.Radical.Windows;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.ComponentModel;

namespace Topics.Radical.Presentation.Autocomplete
{
	[Sample( Title = "Autocomplete", Category = Categories.Presentation )]
	public class AutocompleteViewModel : SampleViewModel
	{
		IList<Person> storage = new List<Person>() 
		{
			new Person(){ FirstName = "Mauro", LastName = "Servienti" },
			new Person(){ FirstName = "Giorgio", LastName = "Formica" },
			new Person(){ FirstName = "Giorgio", LastName = "Gentili" },
			new Person(){ FirstName = "Giorgio", LastName = "Gerosa" },
			new Person(){ FirstName = "Daniele", LastName = "Restelli" }
		};

		public AutocompleteViewModel()
		{
			this.Data = new ObservableCollection<Person>();

			PropertyObserver.For( this )
				.Observe( v => v.Choosen, ( v, s ) =>
				{
					var value = v.Choosen;
				} )
				.Observe( v => v.UserText, ( v, s ) =>
				{
					if( this.UserText.Length >= 3 )
				    {
				        this.Data.Clear();
						var filtered = storage.Where( p => p.FullName.StartsWith( this.UserText, StringComparison.OrdinalIgnoreCase ) );
						foreach( var item in filtered )
						{
							this.Data.Add( item );
						}
				    }
				} );
		}

		public String UserText
		{
			get { return this.GetPropertyValue( () => this.UserText ); }
			set { this.SetPropertyValue( () => this.UserText, value ); }
		}

		public ObservableCollection<Person> Data { get; private set; }

		public Person Choosen
		{
			get { return this.GetPropertyValue( () => this.Choosen ); }
			set { this.SetPropertyValue( () => this.Choosen, value ); }
		}
	}
}