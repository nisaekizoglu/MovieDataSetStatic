using System.Data.SqlClient;
using System.Data;

namespace MovieDataSetStatic.Context
{
    public class DapperContext
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public DapperContext(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("connectionkey");
        }

        public IDbConnection CreateConnection()
        {
            try
            {
                return new SqlConnection(_connectionString);
            }
            catch (Exception ex)
            {
                // Log the exception details for debugging
                Console.WriteLine($"Connection creation failed: {ex.Message}");
                throw;
            }
        }
    }
}
