using System.Data.SqlClient;

namespace Archiver_Beta
{
    public static class SQLconnection
    {
        //a comment
        private static readonly string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=D:\BigProjects\Archiver\Archiver-Beta\Archiver-Beta\Databases\Receipts.mdf;Integrated Security=True";
        private static SqlConnection connection;

        public static SqlConnection GetConnection()
        {
            if (connection == null || connection.State == System.Data.ConnectionState.Closed || connection.State == System.Data.ConnectionState.Broken)
            {
                connection = new SqlConnection(connectionString);
            }
            return connection;
        }
    }
}
