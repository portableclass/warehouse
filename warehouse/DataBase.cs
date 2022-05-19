using System.Data.SqlClient;

namespace warehouse
{
    class DataBase
    {
        SqlConnection sqlConnection = new SqlConnection(@"Data Source=YASH;Initial Catalog=Warehouse;Integrated Security=True");

        public void OpenConnection()
        {
            if (sqlConnection.State == System.Data.ConnectionState.Closed)
            {
                sqlConnection.Open();
            }
        }

        public void CloseConnection()
        {
            if (sqlConnection.State == System.Data.ConnectionState.Open)
            {
                sqlConnection.Close();
            }
        }
        public SqlConnection GetConnection()
        {
            return sqlConnection;
        }
    }
}
