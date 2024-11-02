namespace MMEmergencyCall.Domain.Features.ServiceProvider;

public class ServiceProviderRequestModel
{
    public string ProviderName { get; set; } = null!;

    public int ServiceId { get; set; }

    public string ContactNumber { get; set; } = null!;

    public string? Availability { get; set; }

    public string? TownshipCode { get; set; }
}
