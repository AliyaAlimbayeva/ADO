using Microsoft.Data.SqlClient;
using ProductLib;

namespace ProductLib.Tests
{
    public class TestBase : IDisposable
    {
        private readonly string _connectionString =
            "Data Source=(localdb)\\MSSQLLocalDB;Database = ADODB;Integrated Security=True;Persist Security Info=False;Pooling=False;MultipleActiveResultSets=False;Connect Timeout=5;Encrypt=False;TrustServerCertificate=False";

        public readonly SqlConnection Connection;
        public readonly ConnectedMod connectedMod;
        public readonly DisConnectedMod disconnectedMod;

        public TestBase()
        {
            Connection = new SqlConnection(_connectionString);
            Connection.Open();

            connectedMod = new ConnectedMod(Connection);
            disconnectedMod = new DisConnectedMod(Connection);
        }

        public void Dispose()
        {
            connectedMod.ClearAllData();
            Connection.Dispose();
        }
    }
}