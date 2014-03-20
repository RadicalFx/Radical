using Topics.Radical.ComponentModel;
using Topics.Radical.ComponentModel.ChangeTracking;
using Topics.Radical.ChangeTracking;
using Topics.Radical.Observers;
using Topics.Radical.Windows.Input;
using System.Windows.Input;

namespace Topics.Radical.Presentation.Memento.SimpleGraph
{
	[Sample( Title = "Simple Graph", Category = Categories.Memento )]
	public class SimpleGraphViewModel : SampleViewModel
	{
		readonly IChangeTrackingService service = new ChangeTrackingService();

		public SimpleGraphViewModel()
		{
			var observer = MementoObserver.Monitor( this.service );

			this.UndoCommand = DelegateCommand.Create()
				.OnCanExecute( o => this.service.CanUndo )
				.OnExecute( o => this.service.Undo() )
				.AddMonitor( observer );

			this.RedoCommand = DelegateCommand.Create()
				.OnCanExecute( o => this.service.CanRedo )
				.OnExecute( o => this.service.Redo() )
				.AddMonitor( observer );

			var person = new Person()
			{
				FirstName = "Mauro",
				LastName = "Servienti"
			};

			var entity = new PersonViewModel();

			this.service.Attach( entity );

			entity.Initialize( person, false );

			this.Entity = entity;
		}

		private PersonViewModel _entity = null;
		public PersonViewModel Entity
		{
			get { return this._entity; }
			private set
			{
				if( value != this.Entity )
				{
					this._entity = value;
					this.OnPropertyChanged( () => this.Entity );
				}
			}
		}

		public ICommand UndoCommand { get; private set; }

		public ICommand RedoCommand { get; private set; }
	}
}