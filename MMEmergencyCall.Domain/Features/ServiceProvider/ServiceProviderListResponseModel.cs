namespace MMEmergencyCall.Domain.Features.ServiceProvider;

public class ServiceProviderListResponseModel
{
    public Result<List<Databases.AppDbContextModels.ServiceProvider>> Result { get; set; }
    public ServiceProviderListResponseModel(Result<List<Databases.AppDbContextModels.ServiceProvider>> result)
    {
        Result = result;
    }
}
