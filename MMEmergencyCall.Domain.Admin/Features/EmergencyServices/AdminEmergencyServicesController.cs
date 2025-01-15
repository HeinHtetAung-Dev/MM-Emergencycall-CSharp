namespace MMEmergencyCall.Domain.Admin.Features.EmergencyServices;

[Route("api/Admin/EmergencyServices")]
[AdminAuthorize]
[ApiController]
public class AdminEmergencyServicesController : BaseController
{
	private readonly AdminEmergencyServicesService _adminEmergencyServicesService;

	public AdminEmergencyServicesController(AdminEmergencyServicesService emergencyServiceService)
	{
		_adminEmergencyServicesService = emergencyServiceService;
	}

	[HttpGet("pageNo/{pageNo}/pageSize/{pageSize}")]
	public async Task<IActionResult> GetEmergencyServicesByStatusAsync(
		int pageNo,
		int pageSize,
		string? serviceStatus
	)
	{
		var response = await _adminEmergencyServicesService.GetEmergencyServicesByStatusAsync(
			serviceStatus
			pageNo,
			pageSize,
		);

		return Execute(response);
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

	[HttpPost]
	public async Task<IActionResult> CreateEmergencyServiceAsync(
		AdminEmergencyServicesRequestModel request
	)
	{
		var currentUserId = HttpContext.GetCurrentAdminId();

		if (!currentUserId.HasValue)
		{
			return Unauthorized("Unauthorized Request");
		}

		var userId = Convert.ToInt32(currentUserId);

		var response = await _adminEmergencyServicesService.CreateEmergencyServiceAsync(
			userId,
			request
		);
		return Execute(response);
	}

	[HttpDelete("{id}")]
	public async Task<IActionResult> DeleteEmergencyServiceAsync(int id)
	{
		var currentAdminId = HttpContext.GetCurrentAdminId();

		if (currentAdminId is null)
		{
			return Unauthorized("Unauthorized Request");
		}

		var model = await _adminEmergencyServicesService.DeleteEmergencyServiceStatusAsync(id);

		return Execute(model);
	}

	[HttpPatch("update/{id}")]
	public async Task<IActionResult> UpdateEmergencyServiceAsync(
		int id, AdminEmergencyServicesRequestModel requestModel
	)
	{
		var currentAdminId = HttpContext.GetCurrentAdminId();

		if (currentAdminId is null)
		{
			return Unauthorized("Unauthorized Request");
		}

		var responseModel = await _adminEmergencyServicesService
			.UpdateEmergencyServiceAsync(id, requestModel);

		return Execute(responseModel);
	}

	[HttpDelete("delete/{id}")]
	public async Task<IActionResult> DeleteEmergencyServiceAsync(
		int id, AdminEmergencyServicesRequestModel requestModel
	)
	{
		var currentAdminId = HttpContext.GetCurrentAdminId();

		if (currentAdminId is null)
		{
			return Unauthorized("Unauthorized Request");
		}

		var responseModel = await _adminEmergencyServicesService
			.DeleteEmergencyServiceAsync(id, requestModel);

		return Execute(responseModel);
	}
}
