namespace MMEmergencyCall.Domain.Client.Features.EmergencyRequests;

public class EmergencyRequestRequestModel
{
    public int ServiceId { get; set; }

    public int? ProviderId { get; set; }

    public DateTime RequestTime { get; set; }

    public DateTime? ResponseTime { get; set; }

    public string? Notes { get; set; }

    public string? TownshipCode { get; set; }
}
