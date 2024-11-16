using Microsoft.AspNetCore.Authorization;
using MMEmergencyCall.Domain.Client.Features.EmergencyServices;
using MMEmergencyCall.Domain.Client.Middlewares;

namespace MMEmergencyCall.Domain.Client.Features.EmergencyRequests;

[Route("api/[controller]")]
[ApiController]
public class EmergencyRequestController : BaseController
{
    private readonly EmergencyRequestService _emergencyRequestService;

    public EmergencyRequestController(EmergencyRequestService emergencyRequestService)
    {
        _emergencyRequestService = emergencyRequestService;
    }

    [HttpGet("pageNo/{pageNo}/pageSize/{pageSize}")]
    public async Task<IActionResult> GetEmergencyRequests(string? userId , string? serviceId, string? providerId,
        string? status, string? townshipCode, int pageNo = 1, int pageSize = 10)
    {
        if (pageNo <= 0 || pageSize <= 0)
        {
            return BadRequest("Page number and page size must be greater than zero.");
        }

        var model = await _emergencyRequestService.GetEmergencyRequests(pageNo, pageSize,userId,serviceId,providerId,status,townshipCode);
        return Execute(model);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetEmergencyRequestById(int id)
    {
        var model = await _emergencyRequestService.GetEmergencyRequestById(id);
        return Execute(model);
    }

    [HttpPost]
    [UserAuthorizeAttribute]
    public async Task<IActionResult> AddEmergencyRequest(EmergencyRequestRequestModel request)
    {
        var model = await _emergencyRequestService.AddEmergencyRequest(request);
        return Execute(model);
    }

    [HttpPut("{id}")]
    [UserAuthorizeAttribute]
    public async Task<IActionResult> UpdateEmergencyRequestStatus(int id, UpdateEmergencyRequestStatusRequest statusRequest)
    {
        var model = await _emergencyRequestService.UpdateEmergencyRequestStatus(id, statusRequest);
        return Execute(model);
    }
}
