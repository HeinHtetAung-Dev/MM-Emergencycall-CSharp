namespace MMEmergencyCall.Domain.Client.Features.EmergencyRequests;

public class EmergencyRequestResponseModel
{
    public Result<EmergencyRequest> Result { get; set; }

    public EmergencyRequestResponseModel(Result<EmergencyRequest> result)
    {
        Result = result;
    }
}
