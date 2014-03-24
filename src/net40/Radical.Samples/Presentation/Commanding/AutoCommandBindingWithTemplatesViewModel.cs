using System;
using Topics.Radical.ComponentModel;
using Topics.Radical.Observers;
using Topics.Radical.Windows;
using System.Collections.Generic;
using Topics.Radical.Model;
using System.Threading.Tasks;
using System.Threading;
using Topics.Radical.Threading;

namespace Topics.Radical.Presentation.Commanding
{
	[Sample( Title = "Auto Command Binding (with Templates)", Category = Categories.Commanding )]
	public class AutoCommandBindingWithTemplatesViewModel : SampleViewModel
	{
		public AutoCommandBindingWithTemplatesViewModel()
		{
			this.Children = new List<Child>()
			{
				new Child( this ){ Id = "foo" },
				new Child( this ){ Id = "bar" }
			};
		}

		public List<Child> Children { get; private set; }

		public String InvokedOn
		{
			get { return this.GetPropertyValue( () => this.InvokedOn ); }
			set { this.SetPropertyValue( () => this.InvokedOn, value ); }
		}
	}

	public class Child : Entity
	{
		AutoCommandBindingWithTemplatesViewModel owner;

		public Child( AutoCommandBindingWithTemplatesViewModel owner )
		{
			this.owner = owner;

			this.Status = "Idle";

			this.GetPropertyMetadata( () => this.IsDoActive )
				.AddCascadeChangeNotifications( () => this.CanDo );
		}

		public String Id { get; set; }

		public String Status
		{
			get { return this.GetPropertyValue( () => this.Status ); }
			set { this.SetPropertyValue( () => this.Status, value ); }
		}

		public Boolean IsDoActive
		{
			get { return this.GetPropertyValue( () => this.IsDoActive ); }
			set { this.SetPropertyValue( () => this.IsDoActive, value ); }
		}

		public Boolean CanDo
		{
			get { return this.IsDoActive; }
		}

		public void Do()
		{
			this.owner.InvokedOn = this.Id;
			this.Status = "Running: " + this.Id;

			//await Task.Factory.StartNew( () =>
			//{
			//    //something long running...
			//    Thread.Sleep( 2000 );
			//} );

			this.Status = "Completed: " + this.Id;
		}
	}
}