using System;
using System.Windows.Threading;
using Topics.Radical.Validation;

namespace Topics.Radical.Windows.Presentation
{
	/// <summary>
	/// Allows to execute and action after a user defined amount of time.
	/// </summary>
	public class Wait
	{
		/// <summary>
		/// A waiter for the Wait infrastucture.
		/// </summary>
		public class Waiter
		{
			DispatcherTimer timer;

			internal Waiter( TimeSpan delay )
			{
				this.timer = new DispatcherTimer()
				{
					Interval = delay
				};

				timer.Tick += ( s, a ) =>
				{
					timer.Stop();
					this.action();
				};
			}

			Action action;

			/// <summary>
			/// After the delay executes the given action.
			/// </summary>
			/// <param name="action">The action.</param>
			public void AndThen( Action action )
			{
				Ensure.That( action ).Named( () => action ).IsNotNull();

				this.action = action;
				this.timer.Start();
			}
		}

		/// <summary>
		/// Waits for the specified delay.
		/// </summary>
		/// <param name="delay">The delay.</param>
		/// <returns>A waiter ready to be configured.</returns>
		public static Waiter For( TimeSpan delay )
		{
			return new Waiter( delay );
		}
	}
}
