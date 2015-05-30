using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Topics.Radical.ComponentModel;
using Topics.Radical.Windows.Behaviors;
using Topics.Radical.Windows.Input;

namespace Topics.Radical.Presentation.DragAndDrop
{

	[Sample( Title = "Drag & Drop", Category = Categories.Behaviors )]
	class DragAndDropSampleViewModel : SampleViewModel
	{
		public DragAndDropSampleViewModel()
		{
			this.LeftPersons = new ObservableCollection<Person>() 
			{
				new Person{ FirstName= "Mauro", LastName= "Servienti" },
				new Person{ FirstName= "Giovanni", LastName= "Rossi" },
				new Person{ FirstName= "Marco", LastName= "Verdi" },
			};
			this.RightPersons = new ObservableCollection<Person>();

            var dd = DelegateCommand.Create()
                .OnCanExecute( state =>
                {
                    var e = ( DragOverArgs )state;

                    this.X = e.Position.X;
                    this.Y = e.Position.Y;

                    return true;
                } )
                .OnExecute( state => 
                {
                    var e = ( DropArgs )state;

                    if( e.Data.GetDataPresent( "left/person" ) )
                    {
                        //dragging from left to right
                        var p = ( Person )e.Data.GetData( "left/person" );
                        this.LeftPersons.Remove( p );
                        this.RightPersons.Add( p );
                        if( this.LeftSelectedPerson == p )
                        {
                            this.LeftSelectedPerson = null;
                        }
                        this.RightSelectedPerson = p;
                    }
                    else if( e.Data.GetDataPresent( "right/person" ) )
                    {
                        //dragging from right to left 
                        var p = ( Person )e.Data.GetData( "right/person" );
                        this.RightPersons.Remove( p );
                        this.LeftPersons.Add( p );
                        if( this.RightSelectedPerson == p )
                        {
                            this.RightSelectedPerson = null;
                        }
                        this.LeftSelectedPerson = p;
                    }
                    else
                    {
                        //skip
                    }
                } );

            this.DropPerson = dd;
		}

        public ICommand DropPerson { get; private set; }

		public ObservableCollection<Person> LeftPersons { get; private set; }
		public Person LeftSelectedPerson
		{
			get { return this.GetPropertyValue( () => this.LeftSelectedPerson ); }
			set { this.SetPropertyValue( () => this.LeftSelectedPerson, value ); }
		}

		public ObservableCollection<Person> RightPersons { get; private set; }
		public Person RightSelectedPerson
		{
			get { return this.GetPropertyValue( () => this.RightSelectedPerson ); }
			set { this.SetPropertyValue( () => this.RightSelectedPerson, value ); }
		}

        public Double X
        {
            get { return this.GetPropertyValue( () => this.X ); }
            private set { this.SetPropertyValue( () => this.X, value ); }
        }

        public Double Y
        {
            get { return this.GetPropertyValue( () => this.Y ); }
            private set { this.SetPropertyValue( () => this.Y, value ); }
        }
	}
}
