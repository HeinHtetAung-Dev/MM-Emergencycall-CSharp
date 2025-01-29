using Microsoft.Data.SqlClient;
using System.Data;

namespace MMEmergencyCall.Databases.Dapper;

public class DapperContext
{
	private readonly SqlConnectionStringBuilder _connectionStringBuilder;
	public DapperContext(string connectionString)
	{
		_connectionStringBuilder = new SqlConnectionStringBuilder(connectionString);
	}

	public IDbConnection CreateConnection()
	{
		IDbConnection dbConnection = new SqlConnection(_connectionStringBuilder.ConnectionString);

		return dbConnection;
	}
}
