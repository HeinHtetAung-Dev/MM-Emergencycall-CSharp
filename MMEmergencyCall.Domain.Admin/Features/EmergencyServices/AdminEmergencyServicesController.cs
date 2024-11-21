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

    [HttpGet("pageNo/{pageNo}/pageSize/{pageSize}")]
    public async Task<IActionResult> GetEmergencyServicesByStatusAsync(
        int pageNo,
        int pageSize,
        string? serviceStatus
    )
    {
        var response = await _adminEmergencyServicesService.GetEmergencyServicesByStatusAsync(
            pageNo = 1,
            pageSize = 10,
            serviceStatus
        );

        return Execute(response);
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> UpdateEmergencyServiceStatusAsync(int id, string serviceStatus)
    {
        var currentAdminId = HttpContext.GetCurrentAdminId();

        if (currentAdminId is null)
        {
            return Unauthorized("Unauthorized Request");
        }

        var userId = Convert.ToInt32(currentAdminId);

        var model = await _adminEmergencyServicesService.UpdateEmergencyServiceStatusAsync(
            id,
            userId,
            serviceStatus
        );

        return Execute(model);
    }

    [HttpPost]
    public async Task<IActionResult> CreateEmergencyServiceAsync(
        AdminEmergencyServicesRequestModel request
    )
    {
        var currentUserId = HttpContext.GetCurrentAdminId();

        if (!currentUserId.HasValue)
        {
            return Unauthorized("Unauthorized Request");
        }

        var userId = Convert.ToInt32(currentUserId);

        var response = await _adminEmergencyServicesService.CreateEmergencyServiceAsync(
            userId,
            request
        );
        return Execute(response);
    }
}
