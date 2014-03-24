using System;
using Topics.Radical.ComponentModel;
using Topics.Radical.Observers;
using Topics.Radical.Windows;
using System.Collections.Generic;
//using Topics.Radical.Windows.Model;
using System.Windows.Data;

namespace Topics.Radical.Presentation.EntityCollectionView
{
	//[Sample( Title = "Entity CollectionView", Category = Categories.IEntityView )]
	public class EntityCollectionViewViewModel : SampleViewModel
	{
		public EntityCollectionViewViewModel()
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

			//this.Items = new EntityCollectionView<Person>( data );
		}

		public IEntityView Items { get; private set; }
	}
}