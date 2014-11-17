using System;
using System.Collections.Generic;
using Topics.Radical.ComponentModel;
using Topics.Radical.ComponentModel.Windows.Input;
using Topics.Radical.Model;
using Topics.Radical.Windows.Input;

namespace Topics.Radical.Presentation.EntityView
{
	[Sample( Title = "EntityView Custom Property Mapping", Category = Categories.IEntityView )]
	public class CustomPropertyMappingViewModel : SampleViewModel
	{
		string propertyName = "";

		public CustomPropertyMappingViewModel()
		{
            var data = new List<Person>()
            {
                new Person( "topics" )
                {
                    FirstName = "Mauro",
                    LastName = "Servienti"
                },

                new Person( "gioffy" )
                {
                    FirstName = "Giorgio",
                    LastName = "Formica"
                },

                new Person( "imperugo" )
                {
                    FirstName = "Ugo",
                    LastName = "Lattanzi"
                }
            };

            //var data = new List<dynamic>()
            //{
            //    new
            //    {
            //        Username = "topics",
            //        FirstName = "Mauro",
            //        LastName = "Servienti"
            //    },

            //    new 
            //    {
            //        Username = "gioffy",
            //        FirstName = "Giorgio",
            //        LastName = "Formica"
            //    },

            //    new
            //    {
            //        Username = "imperugo",
            //        FirstName = "Ugo",
            //        LastName = "Lattanzi"
            //    }
            //};

			this.ToggleCustomProperty = DelegateCommand.Create()
				.OnExecute( o =>
				{
					var temp = this.Items;
					this.Items = null;
					this.OnPropertyChanged( () => this.Items );

					if( !temp.IsPropertyMappingDefined( this.propertyName ) )
					{
						var prop = temp.AddPropertyMapping<String>( "Nome proprietà", obj =>
						{
							return obj.Item.EntityItem.FirstName + " " + obj.Item.EntityItem.LastName;
						} );

						this.propertyName = prop.Name;
					}
					else
					{
						temp.RemovePropertyMapping( this.propertyName );
					}

					this.Items = temp;
					this.OnPropertyChanged( () => this.Items );
				} );

			this.Items = new EntityView<Person>( data );
			this.Items.AddingNew += ( s, e ) =>
			{
				e.NewItem = new Person( "--empty--" );
			};

		}

		public IEntityView<Person> Items { get; private set; }

		public IDelegateCommand ToggleCustomProperty
		{
			get;
			private set;
		}
	}
}