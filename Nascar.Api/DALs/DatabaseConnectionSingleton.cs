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
            return "Server=tcp:iamnascarapidb.database.windows.net,1433;Initial Catalog=NASCAR;Persist Security Info=False;User ID=DBA;Password=colum*999nascar!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
        }
    }
}
