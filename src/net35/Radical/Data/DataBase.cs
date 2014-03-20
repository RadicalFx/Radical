namespace Topics.Radical.Data
{
	using System;
	using System.Collections.Generic;
	using System.Data;
	using System.Data.Common;
	using Topics.Radical;
	using Topics.Radical.Validation;

	/// <summary>
	/// A default abstract implementation of the IDataBase interface.
	/// </summary>
	public class DataBase : IDataBase
	{
		readonly List<IDataParameter> parameters = new List<IDataParameter>();

		DbProviderFactory providerFactory;
		DbConnectionWrapper connection;

		String connectionString = null;
		String commandText = null;
		CommandType commandType = CommandType.StoredProcedure;

		/// <summary>
		/// Gets the connection to the database.
		/// </summary>
		/// <returns>An IDbConnection instance.</returns>
		protected virtual IDbConnection GetConnection()
		{
			if( this.connection != null )
			{
				return this.connection;
			}
			else if( this.providerFactory != null )
			{
				IDbConnection conn = this.providerFactory.CreateConnection();
				conn.ConnectionString = this.connectionString;

				return conn;
			}
			else
			{
				throw new InvalidOperationException();
			}
		}

		/// <summary>
		/// Creates the command.
		/// </summary>
		/// <param name="connection">The connection to create the command for.</param>
		/// <returns>An instance of an IDbCommand.</returns>
		protected virtual IDbCommand CreateCommand( IDbConnection connection )
		{
			Ensure.That( connection).Named( "connection" ).IsNotNull();

			IDbCommand command = connection.CreateCommand();
			command.CommandText = this.commandText;
			command.CommandType = this.commandType;

			this.parameters.ForEach( item => command.Parameters.Add( item ) );

			return command;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="DataBase"/> class.
		/// </summary>
		/// <param name="providerName">Name of the provider.</param>
		/// <param name="connectionString">The connection string.</param>
		public DataBase( String providerName, String connectionString )
		{
			Ensure.That( providerName )
				.Named( "providerName" )
				.IsNotNullNorEmpty();

			Ensure.That( connectionString )
				.Named( "connectionString" )
				.IsNotNullNorEmpty();

			this.providerFactory = DbProviderFactories.GetFactory( providerName );
			this.connectionString = connectionString;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="DataBase"/> class.
		/// </summary>
		/// <param name="providerName">Name of the provider.</param>
		/// <param name="connectionString">The connection string.</param>
		/// <param name="defaultCommandType">Default type of the command.</param>
		public DataBase( String providerName, String connectionString, CommandType defaultCommandType )
			: this( providerName, connectionString )
		{
			defaultCommandType.EnsureIsDefined();

			this.commandType = defaultCommandType;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="DataBase"/> class.
		/// </summary>
		/// <param name="connection">The connection.</param>
		public DataBase( DbConnection connection )
		{
			Ensure.That( connection ).Named( "connection" ).IsNotNull();

			this.connection = new DbConnectionWrapper( connection );
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="DataBase"/> class.
		/// </summary>
		/// <param name="connection">The connection.</param>
		/// <param name="defaultCommandType">Default type of the command.</param>
		public DataBase( DbConnection connection, CommandType defaultCommandType )
			: this( connection )
		{
			defaultCommandType.EnsureIsDefined();
			this.commandType = defaultCommandType;
		}

		/// <summary>
		/// Set the command that will be used against the database.
		/// </summary>
		/// <param name="commandText">The command text.</param>
		/// <returns>
		/// A reference to self to allow a fluent interface usage scenario.
		/// </returns>
		public virtual IDataBase UseCommand( String commandText )
		{
			Ensure.That( commandText )
				.Named( "commandText" )
				.IsNotNull()
				.IsNotEmpty();

			this.commandText = commandText;
			return this;
		}

		/// <summary>
		/// Specify the type of the command that will be sent
		/// to the underlying database. The default value is StoredProcedure.
		/// </summary>
		/// <param name="commandType">Type of the command.</param>
		/// <returns>
		/// A reference to self to allow a fluent interface usage scenario.
		/// </returns>
		public virtual IDataBase As( CommandType commandType )
		{
			commandType.EnsureIsDefined();

			this.commandType = commandType;
			return this;
		}

		/// <summary>
		/// Adds the given parameter to the parameters that will
		/// be passed to the command.
		/// </summary>
		/// <param name="parameter">The parameter to add.</param>
		/// <returns>
		/// A reference to self to allow a fluent interface usage scenario.
		/// </returns>
		public virtual IDataBase AddParameter( IDataParameter parameter )
		{
			Ensure.That( parameter )
				.Named( "parameter" )
				.IsNotNull();

			this.parameters.Add( parameter );
			return this;
		}

		/// <summary>
		/// Adds the given list of parameters.
		/// </summary>
		/// <param name="range">The range of parameters to add.</param>
		/// <returns>
		/// A reference to self to allow a fluent interface usage scenario.
		/// </returns>
		public virtual IDataBase AddParameterRange( IEnumerable<IDataParameter> range )
		{
			Ensure.That( range ).Named( "range" ).IsNotNull();

			this.parameters.AddRange( range );
			return this;
		}

		public virtual IDataBase AddParameterRange( IDataParameterCollection range )
		{
			Ensure.That( range ).Named( "range" ).IsNotNull();

			foreach( IDataParameter el in range )
			{
				this.parameters.Add( el );
			}

			return this;
		}

		/// <summary>
		/// Executes the given command text and builds a strongly typed data reader.
		/// </summary>
		/// <typeparam name="T">The type of the data reader to return.</typeparam>
		/// <returns>
		/// An forward/read-only cursor for reading from the datasource.
		/// </returns>
		public T ExecuteReader<T>() where T : class, IDataReader
		{
			return ( T )this.ExecuteReader();
		}

		/// <summary>
		/// Executes the given command text and builds an IDataReader.
		/// </summary>
		/// <returns>
		/// An forward/read-only cursor for reading from the datasource.
		/// </returns>
		public virtual IDataReader ExecuteReader()
		{
			IDbConnection connection = null;
			try
			{
				connection = this.GetConnection();
				using( IDbCommand command = this.CreateCommand( connection ) )
				{
					connection.Open();
					if( connection is DbConnectionWrapper )
					{
						return command.ExecuteReader();
					}
					else
					{
						return command.ExecuteReader( CommandBehavior.CloseConnection );
					}
				}
			}
			catch
			{
				if( connection != null )
				{
					if( connection.State == ConnectionState.Open )
					{
						connection.Close();
					}

					connection.Dispose();
				}

				throw;
			}
		}

		/// <summary>
		/// Executes the query, and returns the first column of the first
		/// row in the resultset returned by the query. Extra columns or
		/// rows are ignored.
		/// </summary>
		/// <returns>
		/// The value of the first column of the first row in the resultset.
		/// </returns>
		public virtual Object ExecuteScalar()
		{
			using( IDbConnection connection = this.GetConnection() )
			{
				using( IDbCommand command = this.CreateCommand( connection ) )
				{
					connection.Open();
					return command.ExecuteScalar();
				}
			}
		}

		public T ExecuteScalar<T>()
		{
			Object result = this.ExecuteScalar();
			if( result is T )
			{
				return ( T )result;
			}

			return default( T );
		}

		/// <summary>
		/// Executes an SQL statement against the Connection object of a
		/// .NET Framework data provider, and returns the number of rows affected.
		/// </summary>
		/// <returns>
		/// The number of rows affected by the command.
		/// </returns>
		public virtual Int32 ExecuteNonQuery()
		{
			using( IDbConnection connection = this.GetConnection() )
			{
				using( IDbCommand command = this.CreateCommand( connection ) )
				{
					connection.Open();
					return command.ExecuteNonQuery();
				}
			}
		}
	}
}
