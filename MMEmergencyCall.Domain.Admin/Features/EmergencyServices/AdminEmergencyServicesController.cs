using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MMEmergencyCall.Domain.Admin.Middlewares;

namespace MMEmergencyCall.Domain.Admin.Features.EmergencyServices;

[Route("api/Admin/EmergencyServices")]
[AdminAuthorizeAttribute]
[ApiController]
public class AdminEmergencyServicesController : BaseController
{
    private readonly AdminEmergencyServicesService _adminEmergencyServicesService;

    public AdminEmergencyServicesController(AdminEmergencyServicesService emergencyServiceService)
    {
        _adminEmergencyServicesService = emergencyServiceService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllEmergencyServicesAsync()
    {
        var response =
            await _adminEmergencyServicesService.GetEmergencyServicesByStatusAsync(
                string.Empty
            );

        return Execute(response);
    }

    [HttpGet("ServiceStatus/{pageNo}/{pageSize}")]
    [HttpGet("ServiceStatus/pageNo/{pageNo}/pageSize/{pageSize}")]

    [HttpGet("ServiceStatus/{serviceStatus}")]
    [HttpGet("ServiceStatus/{serviceStatus}/{pageNo}/{pageSize}")]
    [HttpGet("ServiceStatus/{serviceStatus}/pageNo/{pageNo}/pageSize/{pageSize}")]
    public async Task<IActionResult> GetEmergencyServicesByStatusAsync(
        string? serviceStatus,
        int pageNo = 1,
        int pageSize = 10
    )
    {
        var response =
            await _adminEmergencyServicesService.GetEmergencyServicesByStatusAsync(
                serviceStatus,
                pageNo,
                pageSize
            );

        return Execute(response);
    }
}
