using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Topics.Radical.Windows.Presentation;

namespace TestGridView.Model
{
	class CLIENTIRADICAL : AbstractViewModel
	{
		public int IDR
		{
			get
			{
				return this.GetPropertyValue( () => this.IDR );
			}
			set
			{
				this.SetPropertyValue( () => this.IDR, value );
			}
		}

		public string NameR
		{
			get
			{
				return this.GetPropertyValue( () => this.NameR );
			}
			set
			{
				this.SetPropertyValue( () => this.NameR, value );
			}
		}

		public CLIENTIRADICAL( int iDR, string nameR )
		{
			this.SetInitialPropertyValue( () => this.IDR, iDR );
			this.SetInitialPropertyValue( () => this.NameR, nameR );
		}

	}
}
