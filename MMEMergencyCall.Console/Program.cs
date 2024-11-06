//using MMEmergencyCall.Console.Features;

//string connectionString = "Server=.;Database=MMEmergencyCall;User Id=sa;Password=sasa@123;TrustServerCertificate=True;";  // Update with your actual connection string
//int rowCount = 100;  // Number of rows to insert

//// Initialize the service with the connection string
//ServiceProviderService serviceProvidersService = new ServiceProviderService(connectionString);

//// Seed the ServiceProviders table
//serviceProvidersService.SeedServiceProviders(rowCount);

//Console.WriteLine($"{rowCount} rows successfully inserted into ServiceProviders.");

//// Initialize the EmergencyService with the connection string
//EmergencyService emergencyService = new EmergencyService(connectionString);

//// Seed the EmergencyServices table
//emergencyService.SeedEmergencyServices(rowCount);

//Console.WriteLine($"{rowCount} rows successfully inserted into EmergencyServices.");

//// Initialize the EmergencyRequest with the connection string
//EmergencyRequestService emergencyRequestService = new EmergencyRequestService(connectionString);

//// Seed the EmergencyRequests table
//emergencyRequestService.SeedEmergencyRequests(rowCount);

//Console.WriteLine($"{rowCount} rows successfully inserted into EmergencyRequests.");

//Console.ReadLine();


using Effortless.Net.Encryption;
using System.Security.Cryptography;
using System;
using System.Security.Cryptography;
using MMEmergencyCall.Shared;

//// Generate a 32-byte key
//byte[] key = new byte[32];
//using (var rng = RandomNumberGenerator.Create())
//{
//    rng.GetBytes(key);
//}

//// Generate a 16-byte IV
//byte[] iv = new byte[16];
//using (var rng = RandomNumberGenerator.Create())
//{
//    rng.GetBytes(iv);
//}

//// Convert to Base64 strings for easy representation
//string keyBase64 = Convert.ToBase64String(key);
//string ivBase64 = Convert.ToBase64String(iv);

//Console.WriteLine($"Key: {keyBase64}");
//Console.WriteLine($"IV: {ivBase64}");

string message = "Hello".ToEncrypt();
string message2 = message.ToDecrypt();

Console.WriteLine(message);
Console.WriteLine(message2);

Console.ReadLine();
