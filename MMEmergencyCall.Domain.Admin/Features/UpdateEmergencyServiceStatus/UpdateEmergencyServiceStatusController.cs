namespace MMEmergencyCall.Domain.Admin.Features.UpdateEmergencyServiceStatus;

[Route("api/Admin/UpdateEmergencyServiceStatus")]
[AdminAuthorize]
[ApiController]
public class UpdateEmergencyServiceStatusController: BaseController
{
	private readonly UpdateEmergencyServiceStatusService _updateEmergencyServiceStatusService;

	public UpdateEmergencyServiceStatusController(UpdateEmergencyServiceStatusService updateEmergencyServiceStatusService)
	{
		_updateEmergencyServiceStatusService = updateEmergencyServiceStatusService;
	}

	[HttpPatch("{id}")]
	public async Task<IActionResult> UpdateEmergencyServiceStatusAsync(int id, string serviceStatus)
	{
		var model = await _updateEmergencyServiceStatusService.UpdateEmergencyServiceStatusAsync(
			id,
			serviceStatus
		);

		return Execute(model);
	}
}
