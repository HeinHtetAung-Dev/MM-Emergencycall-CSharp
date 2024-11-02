using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MMEmergencyCall.Domain.Features.ServiceProvider;

namespace MMEmergencyCall.Api.Endpoints.ServiceProviders;

[Route("api/[controller]")]
[ApiController]
public class ServiceProviderController : ControllerBase
{
    private readonly ServiceProviderService _serviceProviderService;
    
    public ServiceProviderController(ServiceProviderService serviceProviderService)
    {
        _serviceProviderService = serviceProviderService;
    }

    [HttpGet]
    public async Task<IActionResult> GetServiceProviders()
    {
        var response = await _serviceProviderService.GetServiceProviders();
        return Ok(response);
    }
}
