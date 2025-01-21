
namespace MMEmergencyCall.Domain.Admin.Features.DeleteEmergencyService;

[Route("api/admin/DeleteEmergencyService")]
public class DeleteEmergencyServiceController : BaseController
{
	private readonly DeleteEmergencyServiceService _deleteEmergencyServicesService;

	public DeleteEmergencyServiceController(DeleteEmergencyServiceService deleteEmergencyServiceService)
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

		var responseModel = await _deleteEmergencyServicesService
			.DeleteEmergencyServiceAsync(id);

		return Execute(responseModel);
	}
}
