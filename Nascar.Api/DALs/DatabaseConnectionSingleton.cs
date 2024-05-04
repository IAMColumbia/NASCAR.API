using System.Data.SqlClient;

namespace Nascar.Api.DALs
{
    public class DatabaseConnectionSingleton
    {
        private static DatabaseConnectionSingleton? instance;

        //use this to reference the DB connection
        private static SqlConnectionStringBuilder? connectionBuilder;

        //make sure there is only one instance of the reference to the database
        public static DatabaseConnectionSingleton Instance()
        {
            if (instance == null)
            {
                instance = new DatabaseConnectionSingleton();
                connectionBuilder = new SqlConnectionStringBuilder();
            }
            return instance;
        }

        private DatabaseConnectionSingleton() { }

        //Create the reference that will be used to connect to the db
        public string PrepareDBConnection()
        {
            connectionBuilder!.DataSource = $"iamnascarapidb.database.windows.net"; //add db connection string here
            connectionBuilder.IntegratedSecurity = true;
            connectionBuilder.InitialCatalog = $"NASCAR";

            //The string that is associated and will be used to reference/represent the DB connection
            return connectionBuilder.ConnectionString;
        }
    }
}
