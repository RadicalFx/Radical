namespace Topics.Radical.Data
{
    using System;
    using System.Data.Common;

    public interface IDataBaseProvider
    {
        IDataBase ConnectingTo( String connectionString );

        IDataBase ConnectingTo( DbConnection connection );
    }
}
