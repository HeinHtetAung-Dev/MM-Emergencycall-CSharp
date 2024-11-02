namespace MMEmergencyCall.Databases.AppDbContextModels;

public partial class ServiceProvider
{
    public int ProviderId { get; set; }

    public string ProviderName { get; set; } = null!;

    public int ServiceId { get; set; }

    public string ContactNumber { get; set; } = null!;

    public string? Availability { get; set; }

    public string? TownshipCode { get; set; }
}
