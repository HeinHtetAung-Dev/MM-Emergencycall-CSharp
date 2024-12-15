using System.ComponentModel.DataAnnotations;

namespace MMEmergencyCall.Domain.Admin.Features.EmergencyServices;

public class AdminEmergencyServicesRequestModel
{
    public string ServiceGroup { get; set; } = null!;

    public string ServiceType { get; set; } = null!;

    public string ServiceName { get; set; } = null!;

    public string PhoneNumber { get; set; } = null!;

    public string? Location { get; set; }

    [MaxLength(1)]
    public string? Availability { get; set; }

    public string? TownshipCode { get; set; }
}
