namespace Topics.Radical.Data
{
    using System;
    using System.Collections.Generic;
    using System.Data;

    /// <summary>
    /// Abstract the access logic to a database exposing a fluent
    /// interface to execute database commands, perform operations
    /// and read data.
    /// </summary>
    public interface IDataBase
    {
        /// <summary>
        /// Adds the given parameter to the parameters that will 
        /// be passed to the command.
        /// </summary>
        /// <param name="parameter">The parameter to add.</param>
        /// <returns>A reference to self to allow a fluent interface usage scenario.</returns>
        IDataBase AddParameter( IDataParameter parameter );

        /// <summary>
        /// Adds the given list of parameters.
        /// </summary>
        /// <param name="range">The range of parameters to add.</param>
        /// <returns>A reference to self to allow a fluent interface usage scenario.</returns>
        IDataBase AddParameterRange( IEnumerable<IDataParameter> range );

        /// <summary>
        /// Adds the given list of parameters.
        /// </summary>
        /// <param name="range">The range of parameters to add.</param>
        /// <returns>A reference to self to allow a fluent interface usage scenario.</returns>
        IDataBase AddParameterRange( IDataParameterCollection range );

        /// <summary>
        /// Set the command that will be used against the database.
        /// </summary>
        /// <param name="commandText">The command text.</param>
        /// <returns>A reference to self to allow a fluent interface usage scenario.</returns>
        IDataBase UseCommand( String commandText );

        ///// <summary>
        ///// Set the connection string to use to connect to the database.
        ///// </summary>
        ///// <param name="connectionString">The connection string.</param>
        ///// <returns>A reference to self to allow a fluent interface usage scenario.</returns>
        //IDataBase On( String connectionString );

        ///// <summary>
        ///// Set the connection to use to connect to the database.
        ///// </summary>
        ///// <param name="connectionString">The connection.</param>
        ///// <returns>A reference to self to allow a fluent interface usage scenario.</returns>
        //IDataBase On( DbConnection connection );
        
        /// <summary>
        /// Specify the type of the command that will be sent 
        /// to the underlying database. The default value is StoredProcedure.
        /// </summary>
        /// <param name="commandType">Type of the command.</param>
        /// <returns>A reference to self to allow a fluent interface usage scenario.</returns>
        IDataBase As( CommandType commandType );

        /// <summary>
        /// Executes an SQL statement against the Connection object of a 
        /// .NET Framework data provider, and returns the number of rows affected.
        /// </summary>
        /// <returns>The number of rows affected by the command.</returns>
        Int32 ExecuteNonQuery();

        /// <summary>
        /// Executes the given command text and builds an IDataReader.
        /// </summary>
        /// <returns>An forward/read-only cursor for reading from the datasource.</returns>
        IDataReader ExecuteReader();

        /// <summary>
        /// Executes the given command text and builds a strongly typed data reader.
        /// </summary>
        /// <typeparam name="T">The type of the data reader to return.</typeparam>
        /// <returns>An forward/read-only cursor for reading from the datasource.</returns>
        T ExecuteReader<T>() where T : class, IDataReader;

        /// <summary>
        /// Executes the query, and returns the first column of the first 
        /// row in the resultset returned by the query. Extra columns or 
        /// rows are ignored.
        /// </summary>
        /// <returns>The value of the first column of the first row in the resultset.</returns>
        Object ExecuteScalar();

        /// <summary>
        /// Executes the query, and returns the first column of the first 
        /// row in the resultset returned by the query. Extra columns or 
        /// rows are ignored.
        /// </summary>
        /// <returns>The value of the first column of the first row in the resultset.</returns>
        T ExecuteScalar<T>();
    }
}
