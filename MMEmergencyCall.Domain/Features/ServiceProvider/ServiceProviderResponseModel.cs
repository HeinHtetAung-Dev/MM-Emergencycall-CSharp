namespace MMEmergencyCall.Domain.Features.ServiceProvider;

public class ServiceProviderResponseModel
{
    public Result<List<Databases.AppDbContextModels.ServiceProvider>> Result { get; set; }

    public ServiceProviderResponseModel(Result<List<Databases.AppDbContextModels.ServiceProvider>> result)
    {
        Result = result;
    }
}
