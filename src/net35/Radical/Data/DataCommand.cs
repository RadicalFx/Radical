namespace Topics.Radical.Data
{
	using System;
	using System.Linq;
	using System.Collections.Generic;
	using System.Data;
	using Topics.Radical.Linq;

	[Obsolete( "Use DataBase instead." )]
	public abstract class DataCommand : IDataCommand
	{
		protected abstract IDbConnection OnGetConnection( ref Boolean shouldDisposeConnectio );

		//protected virtual IDbCommand OnGetCommand( IDbConnection connection )
		//{
		//    IDbCommand command = this.OnCreateCommand( connection );
		//    this.Parameters.ForEach( prm => command.Parameters.Add( prm ) );

		//    return command;
		//}

		protected abstract IDbCommand OnCreateCommand( IDbConnection connection );

		protected virtual void OnDisposeConnection( IDbConnection connection )
		{
			if( connection != null )
			{
				if( connection.State == ConnectionState.Open )
				{
					connection.Close();
				}

				connection.Dispose();
			}
		}

		#region IDisposable Members

		/// <summary>
		/// Releases unmanaged resources and performs other cleanup operations before the
		/// <see cref="SqlDataCommand"/> is reclaimed by garbage collection.
		/// </summary>
		~DataCommand()
		{
			this.Dispose( false );
		}

		/// <summary>
		/// Releases unmanaged and - optionally - managed resources
		/// </summary>
		/// <param name="disposing"><collection>true</collection> to release both managed and unmanaged resources; <collection>false</collection> to release only unmanaged resources.</param>
		protected virtual void Dispose( Boolean disposing )
		{
			if( disposing )
			{
				/*
				 * Se disposing è 'true' significa che dispose
				 * è stato invocato direttamentente dall'utente
				 * è quindi lecito accedere ai 'field' e ad 
				 * eventuali reference perchè sicuramente Finalize
				 * non è ancora stato chiamato su questi oggetti
				 */
				lock( this )
				{

				}
			}
		}

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		public void Dispose()
		{
			this.Dispose( true );
			GC.SuppressFinalize( this );
		}

		#endregion

		protected String ConnectionString
		{
			get;
			private set;
		}

		protected IList<IDataParameter> Parameters
		{
			get;
			private set;
		}

		protected DataCommand( String connString, String cmdText, CommandType cmdType )
		{
			this.Parameters = new List<IDataParameter>();

			this.ConnectionString = connString;

			this.CommandText = cmdText;
			this.CommandType = cmdType;
		}

		#region IDataCommand Members

		/// <summary>
		/// Executes a non query command.
		/// </summary>
		/// <returns>
		/// The number of records affected by the command.
		/// </returns>
		[Obsolete( "Use DataBase instead." )]
		public Int32 ExecuteNonQuery()
		{
			return this.ExecuteNonQuery( this.CommandText );
		}

		[Obsolete( "Use DataBase instead." )]
		public Int32 ExecuteNonQuery( String commandText )
		{
			return this.OnExecuteNonQuery( commandText );
		}

		protected virtual Int32 OnExecuteNonQuery( String commandText )
		{
			Boolean dispose = true;
			IDbConnection connection = null;
			try
			{
				connection = this.OnGetConnection( ref dispose );
				using( IDbCommand command = this.OnCreateCommand( connection ) )
				{
					return command.ExecuteNonQuery();
				}
			}
			finally
			{
				if( dispose )
				{
					this.OnDisposeConnection( connection );
				}
			}
		}

		[Obsolete( "Use DataBase instead." )]
		public Object ExecuteScalar()
		{
			return this.ExecuteScalar( this.CommandText );
		}

		/// <summary>
		/// Executes a scalar command.
		/// </summary>
		/// <returns>The result of the scalar command.</returns>
		[Obsolete( "Use DataBase instead." )]
		public Object ExecuteScalar( String commandText )
		{
			return this.OnExecuteScalar( commandText );
		}

		protected Object OnExecuteScalar( String commandText )
		{
			Boolean dispose = true;
			IDbConnection connection = null;
			try
			{
				connection = this.OnGetConnection( ref dispose );
				using( IDbCommand command = this.OnCreateCommand( connection ) )
				{
					return command.ExecuteScalar();
				}
			}
			finally
			{
				if( dispose )
				{
					this.OnDisposeConnection( connection );
				}
			}
		}

		/// <summary>
		/// Executes the command and returns a data reader.
		/// </summary>
		/// <returns>A forward only cursor for data reading.</returns>
		[Obsolete( "Use DataBase instead." )]
		IDataReader IDataCommand.ExecuteReader()
		{
			return this.OnExecuteReader( this.CommandText );
		}

		[Obsolete( "Use DataBase instead." )]
		IDataReader IDataCommand.ExecuteReader( String commandText )
		{
			return this.OnExecuteReader( commandText );
		}

		protected IDataReader OnExecuteReader( String commandText )
		{
			Boolean dispose = true;
			CommandBehavior cb = dispose ? CommandBehavior.CloseConnection : CommandBehavior.Default;

			IDbConnection connection = null;
			try
			{
				connection = this.OnGetConnection( ref dispose );
				using( IDbCommand command = this.OnCreateCommand( connection ) )
				{
					return command.ExecuteReader( cb );
				}
			}
			finally
			{
				if( dispose )
				{
					this.OnDisposeConnection( connection );
				}
			}
		}

		[Obsolete( "Use DataBase instead." )]
		IDataCommand IDataCommand.Type( CommandType commandType )
		{
			return this.OnType( commandType );
		}

		protected IDataCommand OnType( CommandType commandType )
		{
			this.CommandType = commandType;
			return this;
		}

		[Obsolete( "Use DataBase instead." )]
		IDataCommand IDataCommand.Command( String commandText )
		{
			return this.OnCommand( commandText);
		}

		protected IDataCommand OnCommand( String commandText )
		{
			this.CommandText= commandText;
			return this;
		}

		/// <summary>
		/// Adds a parameter to this command.
		/// </summary>
		/// <param name="parameter">The parameter.</param>
		/// <returns>
		/// A reference to this command, usefull for fluent interfaces.
		/// </returns>
		[Obsolete( "Use DataBase instead." )]
		IDataCommand IDataCommand.AddParameter( IDataParameter parameter )
		{
			return this.OnAddParameter( parameter );
		}

		protected virtual IDataCommand OnAddParameter( IDataParameter parameter )
		{
			if( parameter == null )
			{
				throw new ArgumentNullException( "parameter" );
			}

			this.Parameters.Add( parameter );
			return this;
		}

		[Obsolete( "Use DataBase instead." )]
		IDataCommand IDataCommand.AddParameterRange( IEnumerable<IDataParameter> range )
		{
			return this.OnAddParameterRange( range );
		}

		protected virtual IDataCommand OnAddParameterRange( IEnumerable<IDataParameter> range )
		{
			if( range == null )
			{
				throw new ArgumentNullException( "range" );
			}

			range.ForEach( prm => this.Parameters.Add( prm ) );
			return this;
		}

		/// <summary>
		/// Gets the command text.
		/// </summary>
		/// <value>The command text.</value>
		[Obsolete( "Use DataBase instead." )]
		public String CommandText
		{
			get;
			private set;
		}

		[Obsolete( "Use DataBase instead." )]
		public CommandType CommandType
		{
			get;
			private set;
		}

		#endregion
	}
}
