namespace MMEmergencyCall.Domain.Features.EmergencyRequests;

public class EmergencyRequestListResponseModel
{
    public Result<List<EmergencyRequest>> Result { get; set; }

    public EmergencyRequestListResponseModel(Result<List<EmergencyRequest>> result)
    {
        Result = result;
    }
}
