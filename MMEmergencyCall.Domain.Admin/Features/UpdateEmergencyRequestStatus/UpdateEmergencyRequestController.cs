using MMEmergencyCall.Domain.Admin.Features.EmergencyRequests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMEmergencyCall.Domain.Admin.Features.UpdateEmergencyRequestStatus;

[Route("api/admin/UpdateEmergencyRequest")]
[AdminAuthorize]
[ApiController]
public class UpdateEmergencyRequestController : BaseController
{
	private readonly UpdateEmergencyRequestStatusService _updateEmergencyRequestStatusService;

	public UpdateEmergencyRequestController(UpdateEmergencyRequestStatusService updateEmergencyRequestStatusService)
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
