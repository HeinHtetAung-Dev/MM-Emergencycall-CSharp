using MMEmergencyCall.Domain.Admin.Features.EmergencyServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMEmergencyCall.Domain.Admin.Features.EmergencyService;

[Route("api/Admin/EmergencyService")]
[AdminAuthorize]
[ApiController]
public class EmergencyRequestController: BaseController
{
	private readonly EmergencyServiceService _emergencyServicesService;

	public EmergencyRequestController(EmergencyServiceService emergencyServiceService)
	{
		_emergencyServicesService = emergencyServiceService;
	}

	[HttpGet("pageNo/{pageNo}/pageSize/{pageSize}")]
	public async Task<IActionResult> GetEmergencyServicesByStatusAsync(
		int pageNo,
		int pageSize,
		string? serviceStatus
	)
	{
		var response = await _emergencyServicesService.GetEmergencyServicesByStatusAsync(
			serviceStatus,
			pageNo,
			pageSize
		);

		return Execute(response);
	}
}
