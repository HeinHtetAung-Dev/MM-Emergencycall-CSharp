using MMEmergencyCall.Shared;

namespace MMEmergencyCall.Domain.Admin.Features.StateRegion;

public class StateRegionResponseModel
{
    public Result<StateRegionModel> Result { get; set; }

    public StateRegionResponseModel(Result<StateRegionModel> result)
    {
        Result = result;
    }
}