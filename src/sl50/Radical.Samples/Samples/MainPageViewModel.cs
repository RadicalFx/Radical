using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Topics.Radical.ComponentModel.ChangeTracking;
using Topics.Radical.Model;
using Topics.Radical.ChangeTracking;
using Topics.Radical.Windows.Input;
using Topics.Radical.Observers;
using Topics.Radical.ComponentModel;

namespace Topics.Radical.Samples
{
public class MainPageViewModel : AbstractViewModel
{
	readonly IChangeTrackingService service = new ChangeTrackingService();

	public MainPageViewModel()
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
