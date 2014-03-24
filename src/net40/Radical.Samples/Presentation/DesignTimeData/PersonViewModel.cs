using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using System.Text;
using Topics.Radical.Model;

namespace Topics.Radical.Presentation.DesignTimeData
{
	//public class Wrapper<T> : IDynamicMetaObjectProvider, INotifyPropertyChanged
	//	where T : class
	//{
	//	public DynamicMetaObject GetMetaObject( System.Linq.Expressions.Expression parameter )
	//	{
	//		throw new NotImplementedException();
	//	}

	//	void Commit() { }
	//}

	public class PersonViewModel : Entity
	{
		public PersonViewModel()
		{
			
		}

		static Strings _localization = new Strings();
		public Strings Localization
		{
			get { return _localization; }
		}

		public String FirstName
		{
			get { return this.GetPropertyValue( () => this.FirstName ); }
			private set { this.SetPropertyValue( () => this.FirstName, value ); }
		}

		public String LastName
		{
			get { return this.GetPropertyValue( () => this.LastName ); }
			set { this.SetPropertyValue( () => this.LastName, value ); }
		}

		public Boolean Degreed
		{
			get { return this.GetPropertyValue( () => this.Degreed ); }
			set { this.SetPropertyValue( () => this.Degreed, value ); }
		}
	}
}
