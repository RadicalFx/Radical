using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Topics.Radical.ComponentModel;

namespace Topics.Radical.Presentation.BusyBehavior
{
	[Sample( Title = "BusyStatus behavior", Category = Categories.Behaviors )]
	class BusyBehaviorSampleViewModel : SampleViewModel
	{
		readonly IDispatcher dispatcher;

		public BusyBehaviorSampleViewModel( IDispatcher dispatcher )
		{
			this.dispatcher = dispatcher;
		}

		public Boolean ThresholdElapsed
		{
			get { return this.GetPropertyValue( () => this.ThresholdElapsed ); }
			private set { this.SetPropertyValue( () => this.ThresholdElapsed, value ); }
		}

		public Boolean IsBusy
		{
			get { return this.GetPropertyValue( () => this.IsBusy ); }
			private set { this.SetPropertyValue( () => this.IsBusy, value ); }
		}

		public String Status
		{
			get { return this.GetPropertyValue( () => this.Status ); }
			private set { this.SetPropertyValue( () => this.Status, value ); }
		}

		Worker w = null;

		public void CancelWork()
		{
			if ( this.w != null )
			{
				lock ( this )
				{
					if ( this.w != null )
					{
						this.w.CancelWork();
					}
				}
			}
		}

		public async void WorkAsync()
		{
			this.IsBusy = true;
			this.Status = "running...";

			this.w = new Worker()
			{
				OnThresholdElapsed = () => this.dispatcher.Dispatch( () => this.ThresholdElapsed = true )
			};

			var r = await this.w.Execute( token =>
			{
				var count = 0;
				while ( count < 15 && !token.IsCancellationRequested )
				{
					++count;
					Thread.Sleep( 1000 );
				}
			} );

			lock ( this )
			{
				this.w = null;
			}

			this.Status = r.Cancelled
				? "cancelled."
				: "completed.";

			this.IsBusy = false;
		}
	}

	class Worker
	{
		public class Result
		{
			public Boolean Cancelled { get; set; }
		}

		CancellationTokenSource cs = null;

		public Worker()
		{
			this.OnThresholdElapsed = () => { };
		}

		public Action OnThresholdElapsed { get; set; }

		public async Task<Result> Execute( Action<CancellationToken> action )
		{
			this.cs = new CancellationTokenSource();
			var token = this.cs.Token;

			var r = await Task.Factory.StartNew( () =>
			{
				var threshold = new System.Timers.Timer( 5000 );
				threshold.AutoReset = false;
				threshold.Elapsed += ( s, e ) => this.OnThresholdElapsed();
				threshold.Start();

				action( token );

				threshold.Stop();

				return new Result() { Cancelled = token.IsCancellationRequested };
			}, cs.Token );

			lock ( this )
			{
				this.cs = null;
			}

			return r;
		}

		public void CancelWork()
		{
			if ( this.cs != null )
			{
				lock ( this )
				{
					if ( this.cs != null )
					{
						cs.Cancel();
					}
				}
			}
		}
	}
}
