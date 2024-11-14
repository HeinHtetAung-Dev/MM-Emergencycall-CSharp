using MMEmergencyCall.Shared;

namespace MMEmergencyCall.Domain.Admin.Features.SignIn;

public class AdminSignInResponseModel
{
    public Result<AdminSignInModel> Result { get; set; }

    public AdminSignInResponseModel(Result<AdminSignInModel> result)
    {
        Result = result;
    }
}