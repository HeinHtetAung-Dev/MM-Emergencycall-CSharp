using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MMEmergencyCall.Domain.Features.EmergencyServices;

namespace MMEmergencyCall.Api.Endpoints.EmergencyService
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmergencyServiceController : ControllerBase
    {
        private readonly EmergencyServiceService _emergencyServiceService;

        public EmergencyServiceController(EmergencyServiceService emergencyServiceService)
        {
            _emergencyServiceService = emergencyServiceService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateEmergencyServiceAsync(
            EmergencyServiceRequestModel requestModel
        )
        {
            var response = await _emergencyServiceService.CreateEmergencyServiceAsync(requestModel);
            return Ok(response);
        }
    }
}
