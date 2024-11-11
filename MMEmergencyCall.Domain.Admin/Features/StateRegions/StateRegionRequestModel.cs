namespace MMEmergencyCall.Domain.Admin.Features.StateRegions;

public class StateRegionRequestModel
{
    public string? StateRegionCode { get; set; } = null!;
    public string? StateRegionNameEn { get; set; } = null!;
    public string? StateRegionNameMm { get; set; }
}