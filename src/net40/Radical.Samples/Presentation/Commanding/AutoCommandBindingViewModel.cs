using System;
using Topics.Radical.ComponentModel;
using Topics.Radical.Observers;
using Topics.Radical.Windows;
using System.Collections.ObjectModel;
using Topics.Radical.Model;
using Topics.Radical.ComponentModel.Windows.Input;

namespace Topics.Radical.Presentation.Commanding
{
	[Sample( Title = "Auto Command Binding", Category = Categories.Commanding )]
	public class AutoCommandBindingViewModel : SampleViewModel
	{
		public class Item : Entity
		{
			public void Execute()
			{
				this.ExecutedOn = String.Format( " {0}: {1}", this.Name, DateTime.Now.ToLongTimeString() );
			}

			public String Name { get; set; }

			public String ExecutedOn
			{
				get { return this.GetPropertyValue( () => this.ExecutedOn ); }
				set { this.SetPropertyValue( () => this.ExecutedOn, value ); }
			}
		}

		Fact _canExecuteWithFact;
		public Fact CanExecuteWithFact
		{
			get
			{
				if( this._canExecuteWithFact == null )
				{
					this._canExecuteWithFact = Fact.Create( o =>
					{
						return this.IsActiveWithFact;
					} )
					.AddMonitor
					(
						PropertyObserver.For( this )
							.Observe( v => v.IsActiveWithFact )
					);
				}

				return this._canExecuteWithFact;
			}
		}

		public void ExecuteWithFact()
		{
			this.ExecutedWithFact = DateTime.Now.ToLongTimeString();
		}

		public String ExecutedWithFact
		{
			get { return this.GetPropertyValue( () => this.ExecutedWithFact ); }
			set { this.SetPropertyValue( () => this.ExecutedWithFact, value ); }
		}

		private Boolean _isActiveWithFact = false;
		public Boolean IsActiveWithFact
		{
			get { return this._isActiveWithFact; }
			set
			{
				if( value != this.IsActiveWithFact )
				{
					this._isActiveWithFact = value;
					this.OnPropertyChanged( () => this.IsActiveWithFact );
				}
			}
		}

		public AutoCommandBindingViewModel()
		{
			this.GetPropertyMetadata( () => this.IsActiveWithBoolean )
				.AddCascadeChangeNotifications( () => this.CanExecuteWithBoolean );

			this.Items = new ObservableCollection<Item>() 
			{
				new Item(){ Name = "Foo"},
				new Item(){ Name = "Bar"},
			};
		}

		public Boolean CanExecuteWithBoolean
		{
			get { return this.IsActiveWithBoolean; }
		}

		[KeyBindingAttribute(System.Windows.Input.Key.B, Modifiers= System.Windows.Input.ModifierKeys.Control)]
		public void ExecuteWithBoolean()
		{
			this.ExecutedWithBoolean = DateTime.Now.ToLongTimeString();
		}

		public String ExecutedWithBoolean
		{
			get { return this.GetPropertyValue( () => this.ExecutedWithBoolean ); }
			set { this.SetPropertyValue( () => this.ExecutedWithBoolean, value ); }
		}

		public Boolean IsActiveWithBoolean
		{
			get { return this.GetPropertyValue( () => this.IsActiveWithBoolean ); }
			set { this.SetPropertyValue( () => this.IsActiveWithBoolean, value ); }
		}

		public ObservableCollection<Item> Items { get; private set; }
	}
}