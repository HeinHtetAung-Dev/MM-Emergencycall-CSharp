using MMEmergencyCall.Domain.Admin.Features.EmergencyServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMEmergencyCall.Domain.Admin.Features.DeleteEmergencyServiceStatus;

[Route("api/admin/DeleteEmergencyServiceStatus")]
[AdminAuthorize]
[ApiController]
public class DeleteEmergencyServiceStatusController : BaseController
{
	private readonly DeleteEmergencyServiceStatusService _deleteEmergencyServicesService;

	public DeleteEmergencyServiceStatusController(DeleteEmergencyServiceStatusService deleteEmergencyServiceService)
	{
		_deleteEmergencyServicesService = deleteEmergencyServiceService;
	}

	[HttpDelete("{id}")]
	public async Task<IActionResult> DeleteEmergencyServiceAsync(int id)
	{
		var currentAdminId = HttpContext.GetCurrentAdminId();

		if (currentAdminId is null)
		{
			return Unauthorized("Unauthorized Request");
		}

		var model = await _deleteEmergencyServicesService.DeleteEmergencyServiceStatusAsync(id);

		return Execute(model);
	}
}
