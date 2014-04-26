using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestGridView.Model;
using Topics.Radical.Windows.Presentation;

namespace TestGridView.Presentation
{
	class MainViewModel : AbstractViewModel
	{
		#region Properties
		public int NumRigDaGen
		{
			get
			{
				return this.GetPropertyValue( () => this.NumRigDaGen );
			}
			set
			{
				this.SetPropertyValue( () => this.NumRigDaGen, value );
			}
		}


		List<CLIENTI> listaClienti;
		public List<CLIENTI> ListaClienti
		{
			get
			{
				return listaClienti;
			}
			set
			{
				if ( listaClienti != value )
				{
					listaClienti = value;
					OnPropertyChanged( () => this.ListaClienti );
				}
			}
		}

		List<CLIENTIRADICAL> listaClientiRadical;
		public List<CLIENTIRADICAL> ListaClientiRadical
		{
			get
			{
				return listaClientiRadical;
			}
			set
			{
				if ( listaClientiRadical != value )
				{
					listaClientiRadical = value;
					OnPropertyChanged( () => this.ListaClientiRadical );
				}
			}
		}


		public DateTime Start { get; set; }

		public string Result
		{
			get
			{
				return this.GetPropertyValue( () => this.Result );
			}
			set
			{
				this.SetPropertyValue( () => this.Result, value );
			}
		}
		public string ResultRadical
		{
			get
			{
				return this.GetPropertyValue( () => this.ResultRadical );
			}
			set
			{
				this.SetPropertyValue( () => this.ResultRadical, value );
			}
		}


		#endregion


		public MainViewModel()
		{
			this.SetInitialPropertyValue( () => this.NumRigDaGen, 2000 );
		}


		#region Method
		public IEnumerable<CLIENTI> LoadClienti()
		{
			return from i in Enumerable.Range( 1, NumRigDaGen )
				   select new CLIENTI( i, "Nome" + i );

		}

		public IEnumerable<CLIENTIRADICAL> LoadClientiRadical()
		{
			return from i in Enumerable.Range( 1, NumRigDaGen )
				   select new CLIENTIRADICAL( i, "Nome" + i );

		}
		#endregion

		#region Commang
		public void GenINot()
		{
			Start = DateTime.Now;
			ListaClienti = LoadClienti().ToList();
			Result = String.Format( "Total time to load: {0} ms",
				Math.Round( ( DateTime.Now - Start ).TotalMilliseconds ) );
		}

		public void GenRad()
		{
			Start = DateTime.Now;
			ListaClientiRadical = LoadClientiRadical().ToList();
			ResultRadical = String.Format( "Total time to load: {0} ms",
				Math.Round( ( DateTime.Now - Start ).TotalMilliseconds ) );
		}
		#endregion
	}
}
