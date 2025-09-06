using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace CoreXNugetPackage.DataAccess.Dapper
{
    public class DapperConnectionHelper
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public DapperConnectionHelper(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration["ConnectionStrings:AppDbContextConnection"] ?? "";
        }

        public IDbConnection CreateSqlConnection() => new SqlConnection(_connectionString);
    }
}
