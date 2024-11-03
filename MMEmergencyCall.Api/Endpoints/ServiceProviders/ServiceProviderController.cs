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

    [HttpGet("{providerId}")]
    public async Task<IActionResult> GetServiceProviderById(int providerId)
    {
        var response = await _serviceProviderService.GetServiceProviderById(providerId);
        return Ok(response);
    }

    [HttpPost]
    public async Task<IActionResult> AddServiceProvider(ServiceProviderRequestModel request)
    {
        var validateResult = ValidateServiceProviderRequestModel(request);
        if (validateResult != null) {
            return validateResult;
        }

        var response = await _serviceProviderService.AddServiceProvider(request);
        return Ok(response);
    }

    [HttpPut("{providerId}")]
    public async Task<IActionResult> UpdateServiceProvider(int providerId,  ServiceProviderRequestModel request)
    {
        var validateResult = ValidateServiceProviderRequestModel(request);
        if (validateResult != null)
        {
            return validateResult;
        }

        var response = await _serviceProviderService.UpdateServiceProvider(providerId, request);
        return Ok(response);
    }

    private IActionResult ValidateServiceProviderRequestModel(ServiceProviderRequestModel request)
    {
        if (request == null)
        {
            return BadRequest("Request model cannot be null");
        }

        if(request.ServiceId < 1)
        {
            return BadRequest("Invalid ServiceId");
        }

        if(request.Availability != "Y" && request.Availability != "N")
        {
            return BadRequest("Availability should be either 'Y' or 'N'");
        }

        return null;
    }
}
