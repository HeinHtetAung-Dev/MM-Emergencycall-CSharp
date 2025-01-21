using MMEmergencyCall.Domain.Admin.Features.EmergencyRequests;

namespace MMEmergencyCall.Domain.Admin.Features.UpdateEmergencyRequestStatus;

[Route("api/admin/[controller]")]
[AdminAuthorize]
[ApiController]
public class UpdateEmergencyRequestStatusController : BaseController
{
	private readonly UpdateEmergencyRequestStatusService _updateEmergencyRequestStatusService;

	public UpdateEmergencyRequestStatusController(UpdateEmergencyRequestStatusService updateEmergencyRequestStatusService)
	{
		_updateEmergencyRequestStatusService = updateEmergencyRequestStatusService;
	}

	[HttpPut("{id}")]
	public async Task<IActionResult> UpdateEmergencyRequestStatus(int id, UpdateEmergencyRequestStatusRequest statusRequest)
	{
		var model = await _updateEmergencyRequestStatusService.UpdateEmergencyRequestStatus(id, statusRequest);
		return Execute(model);
	}
}
