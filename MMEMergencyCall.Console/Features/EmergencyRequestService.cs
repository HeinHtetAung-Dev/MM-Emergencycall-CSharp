using Microsoft.Data.SqlClient;

namespace MMEmergencyCall.Console.Features;

public class EmergencyRequestService
{
    private readonly string _connectionString;
    private readonly Random _random = new Random();

    public EmergencyRequestService(string connectionString)
    {
        _connectionString = connectionString;
    }

    public void SeedEmergencyRequests(int rowCount)
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            connection.Open();

            for (int i = 1; i <= rowCount; i++)
            {
                string townshipCode = GetRandomTownshipCode(connection);  // Fetch a random township code
                string emergencyType = GetRandomEmergencyType();          // Fetch a random emergency type
                string emergencyDetails = GetRandomEmergencyDetails(emergencyType); // Fetch random emergency details
                string userPhoneNumber = $"09-{_random.Next(100000000, 999999999)}"; // Random user phone number

                // Insert data into EmergencyRequests table
                InsertEmergencyRequest(connection, emergencyType, emergencyDetails, userPhoneNumber, townshipCode);
            }
        }
    }

    private void InsertEmergencyRequest(SqlConnection connection, string emergencyType, string emergencyDetails, string userPhoneNumber, string townshipCode)
    {
        string sql = "INSERT INTO [dbo].[EmergencyRequests] (EmergencyType, EmergencyDetails, UserPhoneNumber, TownshipCode) " +
                     "VALUES (@EmergencyType, @EmergencyDetails, @UserPhoneNumber, @TownshipCode)";

        using (SqlCommand command = new SqlCommand(sql, connection))
        {
            // Add parameters to the SQL command
            command.Parameters.AddWithValue("@EmergencyType", emergencyType);
            command.Parameters.AddWithValue("@EmergencyDetails", emergencyDetails);
            command.Parameters.AddWithValue("@UserPhoneNumber", userPhoneNumber);
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

    private string GetRandomEmergencyType()
    {
        string[] emergencyTypes = { "Fire", "Medical", "Police", "Rescue", "Accident" };
        return emergencyTypes[_random.Next(emergencyTypes.Length)];
    }

    private string GetRandomEmergencyDetails(string emergencyType)
    {
        switch (emergencyType)
        {
            case "Fire":
                return "Fire reported in the building.";
            case "Medical":
                return "Severe injury reported.";
            case "Police":
                return "Suspicious activity reported.";
            case "Rescue":
                return "Rescue operation needed.";
            case "Accident":
                return "Traffic accident reported.";
            default:
                return "No details available.";
        }
    }
}
