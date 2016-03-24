namespace Topics.Radical.Data
{
    using System.Data;
    using System.Data.Common;

    class DbConnectionWrapper : DbConnection
    {
        /// <summary>
        /// Releases the unmanaged resources used by the <see cref="T:System.ComponentModel.Component"/> and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected override void Dispose( bool disposing )
        {
            base.Dispose( disposing );

            if( disposing && onWrappedStateChange != null )
            {
                this.WrappedConnection.StateChange -= onWrappedStateChange;
            }

            this.WrappedConnection = null;
        }

        StateChangeEventHandler onWrappedStateChange = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="DbConnectionWrapper"/> class.
        /// </summary>
        /// <param name="connection">The connection.</param>
        public DbConnectionWrapper( DbConnection connection )
        {
            onWrappedStateChange = ( s, e ) => { this.OnStateChange( e ); };

            this.WrappedConnection = connection;
            this.WrappedConnection.StateChange += onWrappedStateChange;
        }

        /// <summary>
        /// Gets the time to wait while establishing a connection before terminating the attempt and generating an error.
        /// </summary>
        /// <value></value>
        /// <returns>
        /// The time (in seconds) to wait for a connection to open. The default value is determined by the specific type of connection that you are using.
        /// </returns>
        public override int ConnectionTimeout
        {
            get { return this.WrappedConnection.ConnectionTimeout; }
        }

        /// <summary>
        /// Enlists in the specified transaction.
        /// </summary>
        /// <param name="transaction">A reference to an existing <see cref="T:System.Transactions.Transaction"/> in which to enlist.</param>
        public override void EnlistTransaction( System.Transactions.Transaction transaction )
        {
            this.WrappedConnection.EnlistTransaction( transaction );
        }

        /// <summary>
        /// Returns schema information for the data source of this <see cref="T:System.Data.Common.DbConnection"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Data.DataTable"/> that contains schema information.
        /// </returns>
        /// <PermissionSet>
        ///     <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" PathDiscovery="*AllFiles*"/>
        /// </PermissionSet>
        public override DataTable GetSchema()
        {
            return this.WrappedConnection.GetSchema();
        }

        /// <summary>
        /// Returns schema information for the data source of this <see cref="T:System.Data.Common.DbConnection"/> using the specified string for the schema name.
        /// </summary>
        /// <param name="collectionName">Specifies the name of the schema to return.</param>
        /// <returns>
        /// A <see cref="T:System.Data.DataTable"/> that contains schema information.
        /// </returns>
        /// <PermissionSet>
        ///     <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" PathDiscovery="*AllFiles*"/>
        /// </PermissionSet>
        public override DataTable GetSchema( string collectionName )
        {
            return this.WrappedConnection.GetSchema( collectionName );
        }

        /// <summary>
        /// Returns schema information for the data source of this <see cref="T:System.Data.Common.DbConnection"/> using the specified string for the schema name and the specified string array for the restriction values.
        /// </summary>
        /// <param name="collectionName">Specifies the name of the schema to return.</param>
        /// <param name="restrictionValues">Specifies a set of restriction values for the requested schema.</param>
        /// <returns>
        /// A <see cref="T:System.Data.DataTable"/> that contains schema information.
        /// </returns>
        /// <PermissionSet>
        ///     <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" PathDiscovery="*AllFiles*"/>
        /// </PermissionSet>
        public override DataTable GetSchema( string collectionName, string[] restrictionValues )
        {
            return this.WrappedConnection.GetSchema( collectionName, restrictionValues );
        }

        /// <summary>
        /// Starts a database transaction.
        /// </summary>
        /// <param name="isolationLevel">Specifies the isolation level for the transaction.</param>
        /// <returns>
        /// An object representing the new transaction.
        /// </returns>
        protected override DbTransaction BeginDbTransaction( IsolationLevel isolationLevel )
        {
            return this.WrappedConnection.BeginTransaction( isolationLevel );
        }

        /// <summary>
        /// Changes the current database for an open connection.
        /// </summary>
        /// <param name="databaseName">Specifies the name of the database for the connection to use.</param>
        public override void ChangeDatabase( string databaseName )
        {
            this.WrappedConnection.ChangeDatabase( databaseName );
        }

        /// <summary>
        /// Closes the connection to the database. This is the preferred method of closing any open connection.
        /// </summary>
        /// <exception cref="T:System.Data.Common.DbException">
        /// The connection-level error that occurred while opening the connection.
        /// </exception>
        public override void Close()
        {
            this.WrappedConnection.Close();
        }

        public DbConnection WrappedConnection
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets the string used to open the connection.
        /// </summary>
        /// <value></value>
        /// <returns>
        /// The connection string used to establish the initial connection. The exact contents of the connection string depend on the specific data source for this connection. The default value is an empty string.
        /// </returns>
        public override string ConnectionString
        {
            get { return this.WrappedConnection.ConnectionString; }
            set { this.WrappedConnection.ConnectionString = value; }
        }

        /// <summary>
        /// Creates and returns a <see cref="T:System.Data.Common.DbCommand"/> object associated with the current connection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Data.Common.DbCommand"/> object.
        /// </returns>
        protected override DbCommand CreateDbCommand()
        {
            return this.WrappedConnection.CreateCommand();
        }

        /// <summary>
        /// Gets the name of the database server to which to connect.
        /// </summary>
        /// <value></value>
        /// <returns>
        /// The name of the database server to which to connect. The default value is an empty string.
        /// </returns>
        public override string DataSource
        {
            get { return this.WrappedConnection.DataSource; }
        }

        /// <summary>
        /// Gets the name of the current database after a connection is opened, or the database name specified in the connection string before the connection is opened.
        /// </summary>
        /// <value></value>
        /// <returns>
        /// The name of the current database or the name of the database to be used after a connection is opened. The default value is an empty string.
        /// </returns>
        public override string Database
        {
            get { return this.WrappedConnection.Database; }
        }

        /// <summary>
        /// Opens a database connection with the settings specified by the <see cref="P:System.Data.Common.DbConnection.ConnectionString"/>.
        /// </summary>
        public override void Open()
        {
            if( this.WrappedConnection.State == ConnectionState.Closed )
            {
                this.WrappedConnection.Open();
            }
        }

        /// <summary>
        /// Gets a string that represents the version of the server to which the object is connected.
        /// </summary>
        /// <value></value>
        /// <returns>
        /// The version of the database. The format of the string returned depends on the specific type of connection you are using.
        /// </returns>
        public override string ServerVersion
        {
            get { return this.WrappedConnection.ServerVersion; }
        }

        /// <summary>
        /// Gets a string that describes the state of the connection.
        /// </summary>
        /// <value></value>
        /// <returns>
        /// The state of the connection. The format of the string returned depends on the specific type of connection you are using.
        /// </returns>
        public override ConnectionState State
        {
            get { return this.WrappedConnection.State; }
        }
    }
}
