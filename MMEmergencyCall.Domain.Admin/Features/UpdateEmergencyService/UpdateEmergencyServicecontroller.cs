using MMEmergencyCall.Domain.Admin.Common;

namespace MMEmergencyCall.Domain.Admin.Features.UpdateEmergencyService;

[Route("api/admin/UpdateEmergencyService")]
public class UpdateEmergencyServicecontroller : BaseController
{
	private readonly UpdateEmergencyServiceService _updateEmergencyServicesService;

	public UpdateEmergencyServicecontroller(UpdateEmergencyServiceService updateEmergencyServiceService)
	{
		_updateEmergencyServicesService = updateEmergencyServiceService;
	}

	[HttpPatch("{id}")]
	public async Task<IActionResult> UpdateEmergencyServiceAsync(
		int id, EmergencyServiceRequestModel requestModel
	)
	{
		var currentAdminId = HttpContext.GetCurrentAdminId();

		if (currentAdminId is null)
		{
			return Unauthorized("Unauthorized Request");
		}

		var responseModel = await _updateEmergencyServicesService
			.UpdateEmergencyServiceAsync(id, requestModel);

		return Execute(responseModel);
	}
}
