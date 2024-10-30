using Microsoft.Data.SqlClient;
using System;

namespace MMEmergencyCall.Console.Features;

public class ServiceProviderService
{
    private readonly string _connectionString;
    private readonly Random _random = new Random();

    public ServiceProviderService(string connectionString)
    {
        _connectionString = connectionString;
    }

    public void SeedServiceProviders(int rowCount)
    {
        using var connection = new SqlConnection(_connectionString);
        connection.Open();

        for (int i = 1; i <= rowCount; i++)
        {
            // Generate random values
            string providerName = $"Provider_{i}";
            int serviceId = _random.Next(1, 101);  // ServiceId between 1 and 100
            string contactNumber = "09-" + _random.Next(100000000, 999999999).ToString();  // Random contact number
            string availability = _random.Next(2) == 0 ? "Y" : "N";  // Random availability
            string townshipCode = GetRandomTownshipCode(connection);  // Fetch a random township code

            // Insert data into ServiceProviders table
            InsertServiceProvider(connection, providerName, serviceId, contactNumber, availability, townshipCode);
        }
    }

    private void InsertServiceProvider(SqlConnection connection, string providerName, int serviceId, string contactNumber, string availability, string townshipCode)
    {
        string sql = "INSERT INTO [dbo].[ServiceProviders] (ProviderName, ServiceId, ContactNumber, Availability, TownshipCode) " +
                     "VALUES (@ProviderName, @ServiceId, @ContactNumber, @Availability, @TownshipCode)";

        using (SqlCommand command = new SqlCommand(sql, connection))
        {
            // Add parameters to the SQL command
            command.Parameters.AddWithValue("@ProviderName", providerName);
            command.Parameters.AddWithValue("@ServiceId", serviceId);
            command.Parameters.AddWithValue("@ContactNumber", contactNumber);
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
}
