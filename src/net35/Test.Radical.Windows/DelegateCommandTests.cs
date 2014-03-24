namespace Test.Radical.Windows
{
	using System;
	using System.Linq;
	using Microsoft.VisualStudio.TestTools.UnitTesting;
	using Rhino.Mocks;
	using Topics.Radical.Windows.Input;
	using Topics.Radical.Observers;
	using Topics.Radical.ComponentModel.ChangeTracking;
	using SharpTestsEx;
	using System.ComponentModel;
	using Topics.Radical;

	[TestClass]
	public class DelegateCommandTests
	{
		class TestStub : INotifyPropertyChanged
		{
			public event PropertyChangedEventHandler PropertyChanged;
			void OnPropertyChanged( String name )
			{
				if( this.PropertyChanged != null )
				{
					this.PropertyChanged( this, new PropertyChangedEventArgs( name ) );
				}
			}

			private String _value = null;
			public String Value
			{
				get { return this._value; }
				set
				{
					if( value != this.Value )
					{
						this._value = value;
						this.OnPropertyChanged( "Value" );
					}
				}
			}

			private String _anotherValue = null;
			public String AnotherValue
			{
				get { return this._anotherValue; }
				set
				{
					if( value != this.AnotherValue )
					{
						this._anotherValue = value;
						this.OnPropertyChanged( "AnotherValue" );
					}
				}
			}

			private readonly Observable<String> _text = new Observable<String>();
			public Observable<String> Text
			{
				get { return this._text; }
			}
		}

		[TestMethod]
		public void delegateCommand_trigger_using_mementoMonitor_and_manually_calling_notifyChanged_should_raise_CanExecuteChanged()
		{
			var expected = 1;
			var actual = 0;

			var svc = MockRepository.GenerateStub<IChangeTrackingService>();
			var monitor = new MementoMonitor( svc );

			var target = DelegateCommand.Create().AddMonitor( monitor );
			target.CanExecuteChanged += ( s, e ) => actual++;
			monitor.NotifyChanged();

			actual.Should().Be.EqualTo( expected );
		}

		[TestMethod]
		public void delegateCommand_trigger_using_mementoMonitor_and_triggering_changes_on_the_memento_should_raise_canExecuteChanged()
		{
			var expected = 1;
			var actual = 0;

			var svc = MockRepository.GenerateStub<IChangeTrackingService>();
			var raiser = svc.GetEventRaiser( obj => obj.TrackingServiceStateChanged += null );
			var monitor = new MementoMonitor( svc );

			var target = DelegateCommand.Create().AddMonitor( monitor );
			target.CanExecuteChanged += ( s, e ) => actual++;

			raiser.Raise( svc, EventArgs.Empty );

			actual.Should().Be.EqualTo( expected );
		}

		[TestMethod]
		public void delegateCommand_trigger_using_PropertyObserver_ForAllproperties_should_trigger_canExecuteChanged()
		{
			var expected = 2;
			var actual = 0;

			var stub = new TestStub();

			var target = DelegateCommand.Create().Observe( stub );
			target.CanExecuteChanged += ( s, e ) => actual++;

			stub.Value = "this raises PropertyChanged";
			stub.AnotherValue = "this raises PropertyChanged";

			actual.Should().Be.EqualTo( expected );
		}

		[TestMethod]
		public void delegateCommand_trigger_using_PropertyObserver_For_a_property_should_trigger_canExecuteChanged()
		{
			var expected = 1;
			var actual = 0;

			var stub = new TestStub();

			var target = DelegateCommand.Create().Observe( stub, s => s.Value );
			target.CanExecuteChanged += ( s, e ) => actual++;

			stub.Value = "this raises PropertyChanged";
			stub.AnotherValue = "this raises PropertyChanged";

			actual.Should().Be.EqualTo( expected );
		}
	}
}
