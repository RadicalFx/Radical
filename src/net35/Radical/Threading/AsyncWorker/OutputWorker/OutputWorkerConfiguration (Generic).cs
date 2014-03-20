using System;
using Topics.Radical.Validation;

namespace Topics.Radical.Threading
{
	/// <summary>
	/// Defines an async worker with a result.
	/// </summary>
	/// <typeparam name="TResult">The type of the result.</typeparam>
	public interface IOuputWorker<TResult>
	{
		/// <summary>
		/// Configures the current worker.
		/// </summary>
		/// <param name="cfg">The configuration handler.</param>
		/// <returns>A configured worker ready to be executed.</returns>
		IConfiguredOuputWorker<TResult> Configure( Action<IOutputWorkerConfiguration<TResult>> cfg );

		/// <summary>
		/// Executes the specified async action.
		/// </summary>
		/// <param name="asyncHandler">The async handler.</param>
		/// <returns>
		/// An <see cref="IWorker"/> instance useful to track worker state.
		/// </returns>
		IWorker Execute( Action<IOutputAsyncArgs<TResult>> asyncHandler );

		/// <summary>
		/// Stores the specified async action and return an <see cref="IExecutableWorker"/> that can be run when required.
		/// </summary>
		/// <param name="asyncHandler">The async handler.</param>
		/// <returns>
		/// An <see cref="IExecutableWorker"/> instance that can be run when required and is useful to track worker state.
		/// </returns>
		IExecutableWorker OnExecute( Action<IOutputAsyncArgs<TResult>> asyncHandler );
	}

	/// <summary>
	/// Defines a configured async worker that has a result.
	/// </summary>
	/// <typeparam name="TResult">The type of the result.</typeparam>
	public interface IConfiguredOuputWorker<TResult>
	{
		/// <summary>
		/// Executes the specified async action.
		/// </summary>
		/// <param name="asyncHandler">The async handler.</param>
		/// <returns>
		/// An <see cref="IWorker"/> instance useful to track worker state.
		/// </returns>
		IWorker Execute( Action<IOutputAsyncArgs<TResult>> asyncHandler );
		
		/// <summary>
		/// Stores the specified async action and return an <see cref="IExecutableWorker"/> that can be run when required.
		/// </summary>
		/// <param name="asyncHandler">The async handler.</param>
		/// <returns>
		/// An <see cref="IExecutableWorker"/> instance that can be run when required and is useful to track worker state.
		/// </returns>
		IExecutableWorker OnExecute( Action<IOutputAsyncArgs<TResult>> asyncHandler );
	}

	/// <summary>
	/// Defines the configuration for a worker with a result.
	/// </summary>
	/// <typeparam name="TResult">The type of the result.</typeparam>
	public interface IOutputWorkerConfiguration<TResult> : IWorkerConfiguration
	{
		/// <summary>
		/// Gets or sets the before execution handler.
		/// </summary>
		/// <value>The before execution handler.</value>
		Action<ICancelArgs> Before { get; set; }

		/// <summary>
		/// Gets or sets the after execution handler.
		/// </summary>
		/// <value>The after execution handler.</value>
		Action<IOutputAfterArgs<TResult>> After { get; set; }
	}

	sealed class OutputWorkerConfiguration<TResult> :
		WorkerConfiguration<Object, TResult>,
		IOutputWorkerConfiguration<TResult>
	{
		public OutputWorkerConfiguration( WorkerArgs<Object, TResult> arguments )
			: base( arguments )
		{

		}

		public Action<ICancelArgs> Before
		{
			get;
			set;
		}

		public override bool ExecuteBefore()
		{
			if( this.Before != null )
			{
				this.Before( this.WorkerArguments );
				return this.WorkerArguments.Cancel;
			}

			return false;
		}

		public override void ExecuteAsync()
		{
			( ( Action<IOutputAsyncArgs<TResult>> )this.AsyncHandler )( this.WorkerArguments );
		}

		public Action<IOutputAfterArgs<TResult>> After
		{
			get;
			set;
		}

		public override void ExecuteAfter()
		{
			if( this.After != null )
			{
				this.After( this.WorkerArguments );
			}
		}
	}
}
