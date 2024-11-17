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
    //[HttpGet("ServiceStatus")]
    public async Task<IActionResult> GetAllEmergencyServicesAsync()
    {
        var response = await _adminEmergencyServicesService.GetAllEmergencyServicesAsync();
        return Execute(response);
    }

    [HttpGet("ServiceStatus")]
    [HttpGet("ServiceStatus/{serviceStatus}")]
    public async Task<IActionResult> GetEmergencyServicesByStatus(string? serviceStatus)
    {
        var response = await _adminEmergencyServicesService.GetEmergencyServicesByStatus(
            serviceStatus
        );
        return Ok(response);
    }

    [HttpGet("ServiceStatus/{pageNo}/{pageSize}")]
    [HttpGet("ServiceStatus/{serviceStatus}/{pageNo}/{pageSize}")]
    [HttpGet("ServiceStatus/{serviceStatus}/pageNo/{pageNo}/pageSize/{pageSize}")]
    public async Task<IActionResult> GetEmergencyServicesByStatusPaginationAsync(
        string? serviceStatus,
        int pageNo = 1,
        int pageSize = 10
    )
    {
        var response =
            await _adminEmergencyServicesService.GetEmergencyServicesByStatusPaginationAsync(
                serviceStatus,
                pageNo,
                pageSize
            );

        return Execute(response);
    }
}
