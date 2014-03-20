namespace Topics.Radical.Data
{
	using System;
	using System.Collections.Generic;
	using System.Data;

	/// <summary>
	/// Base interface that incapsulates all the logic
	/// of a data command.
	/// </summary>
	[Obsolete( "Use IDataBase instead." )]
	public interface IDataCommand : IDisposable
	{
		/// <summary>
		/// Adds a parameter to this command.
		/// </summary>
		/// <param name="parameter">The parameter.</param>
		/// <returns>A reference to this command, usefull for fluent interfaces.</returns>
		[Obsolete( "Use IDataBase instead." )]
		IDataCommand AddParameter( IDataParameter parameter );

		/// <summary>
		/// Adds the given parameter array to this command.
		/// </summary>
		/// <param name="range">The range.</param>
		/// <returns>A reference to this command, usefull for fluent interfaces.</returns>
		[Obsolete( "Use IDataBase instead." )]
		IDataCommand AddParameterRange( IEnumerable<IDataParameter> range );

		/// <summary>
		/// Executes a non query command.
		/// </summary>
		/// <returns>The number of records affected by the command.</returns>
		[Obsolete( "Use IDataBase instead." )]
		Int32 ExecuteNonQuery();

		[Obsolete( "Use IDataBase instead." )]
		Int32 ExecuteNonQuery( String commandText );

		/// <summary>
		/// Executes the command and returns a data reader.
		/// </summary>
		/// <returns>A forward only cursor for data reading.</returns>
		[Obsolete( "Use IDataBase instead." )]
		IDataReader ExecuteReader();

		[Obsolete( "Use IDataBase instead." )]
		IDataReader ExecuteReader( String commandText );

		/// <summary>
		/// Executes a scalar command.
		/// </summary>
		/// <returns>The result of the scalar command.</returns>
		[Obsolete( "Use IDataBase instead." )]
		Object ExecuteScalar();

		[Obsolete( "Use IDataBase instead." )]
		Object ExecuteScalar( String commandText );

		/// <summary>
		/// Gets the command text.
		/// </summary>
		/// <value>The command text.</value>
		[Obsolete( "Use IDataBase instead." )]
		String CommandText { get; }

		[Obsolete( "Use IDataBase instead." )]
		IDataCommand Command( String commandText );

		[Obsolete( "Use IDataBase instead." )]
		CommandType CommandType { get; }

		[Obsolete( "Use IDataBase instead." )]
		IDataCommand Type( CommandType commandType );
	}
}
