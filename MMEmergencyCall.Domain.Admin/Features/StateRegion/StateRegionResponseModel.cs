using MMEmergencyCall.Shared;

namespace MMEmergencyCall.Domain.Admin.Features.StateRegion;

public class StateRegionResponseModel
{
    public int StateRegionId { get; set; }
    public string StateRegionCode { get; set; } = null!;
    public string StateRegionNameEn { get; set; } = null!;
    public string? StateRegionNameMm { get; set; }
}