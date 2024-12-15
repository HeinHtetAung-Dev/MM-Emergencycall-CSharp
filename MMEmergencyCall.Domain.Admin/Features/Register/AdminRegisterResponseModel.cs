namespace MMEmergencyCall.Domain.Admin.Features.Register;

public class AdminRegisterResponseModel
{
    public Result<User> Result { get; set; }

    public AdminRegisterResponseModel(Result<User> result)
    {
        Result = result;
    }
}