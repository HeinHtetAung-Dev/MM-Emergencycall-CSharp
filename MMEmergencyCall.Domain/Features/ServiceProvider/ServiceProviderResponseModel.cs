namespace MMEmergencyCall.Domain.Features.ServiceProvider;

public class ServiceProviderResponseModel
{
    public Result<Databases.AppDbContextModels.ServiceProvider> Result { get; set; }

    public ServiceProviderResponseModel(Result<Databases.AppDbContextModels.ServiceProvider> result)
    {
        Result = result;
    }
}
