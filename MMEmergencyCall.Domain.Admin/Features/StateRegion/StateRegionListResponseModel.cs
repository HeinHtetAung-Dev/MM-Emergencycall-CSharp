namespace MMEmergencyCall.Domain.Admin.Features.StateRegion;

public class StateRegionListResponseModel
{
    public List<StateRegionResponseModel> StateRegions { get; set; } = new List<StateRegionResponseModel>();
    public int TotalCount { get; set; }
}