using Microsoft.Data.SqlClient;

namespace MMEmergencyCall.Console.Features;

public class EmergencyService
{
    private readonly string _connectionString;
    private readonly Random _random = new Random();

    public EmergencyService(string connectionString)
    {
        _connectionString = connectionString;
    }

    public void SeedEmergencyServices(int rowCount)
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            connection.Open();

            for (int i = 1; i <= rowCount; i++)
            {
                string townshipCode = GetRandomTownshipCode(connection);  // Fetch a random township code
                string serviceType = GetRandomServiceType(connection);      // Fetch a random service type
                string serviceName = $"{serviceType}_Service_{i}";         // ServiceName
                string phoneNumber = $"09-{_random.Next(100000000, 999999999)}"; // Random phone number
                string location = $"Location_{i}";                          // Location
                string availability = _random.Next(2) == 0 ? "Y" : "N";    // Random availability

                // Insert data into EmergencyServices table
                InsertEmergencyService(connection, serviceType, serviceName, phoneNumber, location, availability, townshipCode);
            }
        }
    }

    private void InsertEmergencyService(SqlConnection connection, string serviceType, string serviceName, string phoneNumber, string location, string availability, string townshipCode)
    {
        string sql = "INSERT INTO [dbo].[EmergencyServices] (ServiceType, ServiceName, PhoneNumber, Location, Availability, TownshipCode) " +
                     "VALUES (@ServiceType, @ServiceName, @PhoneNumber, @Location, @Availability, @TownshipCode)";

        using (SqlCommand command = new SqlCommand(sql, connection))
        {
            // Add parameters to the SQL command
            command.Parameters.AddWithValue("@ServiceType", serviceType);
            command.Parameters.AddWithValue("@ServiceName", serviceName);
            command.Parameters.AddWithValue("@PhoneNumber", phoneNumber);
            command.Parameters.AddWithValue("@Location", location);
            command.Parameters.AddWithValue("@Availability", availability);
            command.Parameters.AddWithValue("@TownshipCode", townshipCode);

            // Execute the SQL command
            command.ExecuteNonQuery();
        }
    }

    private string GetRandomTownshipCode(SqlConnection connection)
    {
        string townshipCode = null;
        string sql = "SELECT TOP 1 TownshipCode FROM Townships ORDER BY NEWID()";

        using (SqlCommand command = new SqlCommand(sql, connection))
        {
            using (SqlDataReader reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    townshipCode = reader.GetString(0);  // Get the TownshipCode value
                }
            }
        }
        return townshipCode;
    }

    private string GetRandomServiceType(SqlConnection connection)
    {
        string serviceType = null;
        string sql = "SELECT TOP 1 Type FROM (VALUES ('Medical'), ('Police'), ('Fire'), ('Rescue'), ('Ambulance')) AS Types(Type) ORDER BY NEWID()";

        using (SqlCommand command = new SqlCommand(sql, connection))
        {
            using (SqlDataReader reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    serviceType = reader.GetString(0);  // Get the ServiceType value
                }
            }
        }
        return serviceType;
    }
}

