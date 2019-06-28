namespace Radical.ComponentModel.QueryModel
{
    /// <summary>
    /// Represents the base interface for all the batch commands engines.
    /// </summary>
    public interface IBatchCommandEngine<TCommand, TProvider> where TCommand : IBatchCommand
    {
        /// <summary>
        /// Executes the specified command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="provider">The provider to use for execution.</param>
        /// <returns>
        /// THe number of affected elements.
        /// </returns>
        int Execute( TCommand command, TProvider provider );
    }
}
