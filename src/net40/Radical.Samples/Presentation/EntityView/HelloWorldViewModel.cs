using System.Collections.Generic;
using Topics.Radical.ComponentModel;
using Topics.Radical.Model;

namespace Topics.Radical.Presentation.EntityView
{
	[Sample( Title = "EntityView Primer", Category = Categories.IEntityView )]
	public class HelloWorldViewModel : SampleViewModel
	{
		public HelloWorldViewModel()
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

			this.Items = new EntityView<Person>( data );
			this.Items.AddingNew += ( s, e ) => 
			{
				e.NewItem = new Person("--empty--");
			};
		}

		public IEntityView<Person> Items { get; private set; }
	}
}