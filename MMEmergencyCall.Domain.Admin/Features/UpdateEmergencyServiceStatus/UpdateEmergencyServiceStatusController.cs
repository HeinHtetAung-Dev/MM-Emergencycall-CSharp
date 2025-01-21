using MMEmergencyCall.Domain.Admin.Features.EmergencyServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMEmergencyCall.Domain.Admin.Features.UpdateEmergencyServiceStatus;

[Route("api/Admin/UpdateEmergencyServiceStatus")]
[AdminAuthorize]
[ApiController]
public class UpdateEmergencyServiceStatusController: BaseController
{
	private readonly AdminEmergencyServicesService _adminEmergencyServicesService;

	public UpdateEmergencyServiceStatusController(AdminEmergencyServicesService emergencyServiceService)
	{
		_adminEmergencyServicesService = emergencyServiceService;
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
			serviceStatus
		);

		return Execute(model);
	}
}
