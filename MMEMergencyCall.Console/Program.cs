using MMEmergencyCall.Console.Features;

string connectionString = "Server=.;Database=MMEmergencyCall;User Id=sa;Password=sasa@123;TrustServerCertificate=True;";  // Update with your actual connection string
int rowCount = 100;  // Number of rows to insert

// Initialize the service with the connection string
ServiceProviderService serviceProvidersService = new ServiceProviderService(connectionString);

// Seed the ServiceProviders table
serviceProvidersService.SeedServiceProviders(rowCount);

Console.WriteLine($"{rowCount} rows successfully inserted into ServiceProviders.");

// Initialize the EmergencyService with the connection string
EmergencyService emergencyService = new EmergencyService(connectionString);

// Seed the EmergencyServices table
emergencyService.SeedEmergencyServices(rowCount);

Console.WriteLine($"{rowCount} rows successfully inserted into EmergencyServices.");

// Initialize the EmergencyRequest with the connection string
EmergencyRequestService emergencyRequestService = new EmergencyRequestService(connectionString);

// Seed the EmergencyRequests table
emergencyRequestService.SeedEmergencyRequests(rowCount);

Console.WriteLine($"{rowCount} rows successfully inserted into EmergencyRequests.");

Console.ReadLine();
