using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MMEmergencyCall.Api.Middlewares;
using MMEmergencyCall.Domain.Client.Features.EmergencyRequests;

namespace MMEmergencyCall.Api.Endpoints.EmergencyRequests;

[Route("api/[controller]")]
[ApiController]
//[CustomAuthorize]
public class EmergencyRequestController : ControllerBase
{
    private readonly EmergencyRequestService _emergencyRequestService;

    public EmergencyRequestController(EmergencyRequestService emergencyRequestService)
    {
        _emergencyRequestService = emergencyRequestService;
    }

    [HttpGet]
    public async Task<IActionResult> GetEmergencyRequests()
    {
        var response = await _emergencyRequestService.GetEmergencyRequests();
        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetEmergencyRequestById(int id)
    {
        var response = await _emergencyRequestService.GetEmergencyRequestById(id);
        return Ok(response);
    }

    [HttpPost]
    public async Task<IActionResult> AddEmergencyRequest(EmergencyRequestRequestModel request)
    {
        var validationResult = ValidateEmergencyRequest(request);
        if (validationResult != null)
        {
            return validationResult;
        }

        var response = await _emergencyRequestService.AddEmergencyRequest(request);
        return Ok(response);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateEmergencyRequest(int id,EmergencyRequestRequestModel request)
    {
        var validationResult = ValidateEmergencyRequest(request);
        if (validationResult != null)
        {
            return validationResult;
        }

        var response = await _emergencyRequestService.UpdateEmergencyRequest(id, request);
        return Ok(response);
    }

    private IActionResult? ValidateEmergencyRequest(EmergencyRequestRequestModel? request)
    {
        if(request is null)
        {
            return BadRequest("Request model cannot be null");
        }
        if(request.UserId < 1)
        {
            return BadRequest("Invalid User Id");
        }
        if (request.ProviderId < 1)
        {
            return BadRequest("Invalid Provider Id");
        }
        if (request.ServiceId < 1)
        {
            return BadRequest("Invalid Service Id");
        }

        if(request.Status != "Pending" && request.Status != "Completed")
        {
            return BadRequest("Status should be either Pending or Completed");
        }

        return null;
    }
}
