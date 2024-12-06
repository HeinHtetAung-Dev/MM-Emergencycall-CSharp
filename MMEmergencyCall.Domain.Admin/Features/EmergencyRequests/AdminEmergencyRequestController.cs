namespace MMEmergencyCall.Domain.Admin.Features.EmergencyRequests;

[Route("api/admin/EmergencyRequest")]
[AdminAuthorizeAttribute]
[ApiController]
public class AdminEmergencyRequestController : BaseController
{
    private readonly AdminEmergencyRequestService _adminEmergencyRequestService;

    public AdminEmergencyRequestController(AdminEmergencyRequestService adminEmergencyRequestService)
    {
        _adminEmergencyRequestService = adminEmergencyRequestService;
    }

    [HttpGet("pageNo/{pageNo}/pageSize/{pageSize}")]
    public async Task<IActionResult> GetEmergencyRequests(int? userId,string? serviceId, string? providerId,
        string? status, string? townshipCode, int pageNo = 1, int pageSize = 10)
    {
        if (pageNo <= 0 || pageSize <= 0)
        {
            return BadRequest("Page number and page size must be greater than zero.");
        }

        var model = await _adminEmergencyRequestService.GetEmergencyRequests
            (pageNo, pageSize, userId, serviceId, providerId, status, townshipCode);
        return Execute(model);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateEmergencyRequestStatus(int id, UpdateEmergencyRequestStatusRequest statusRequest)
    {
        var model = await _adminEmergencyRequestService.UpdateEmergencyRequestStatus(id, statusRequest);
        return Execute(model);
    }
}
