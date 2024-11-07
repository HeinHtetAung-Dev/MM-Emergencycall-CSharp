using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MMEmergencyCall.Domain.User.Features.EmergencyServices;

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

        [HttpGet("{serviceId}")]
        public async Task<IActionResult> GetEmergencyServiceById(int serviceId)
        {
            var response = await _emergencyServiceService.GetEmergencyServiceById(serviceId);
            return Ok(response);
        }

        [HttpGet("ServiceType/{serviceType}")]
        public async Task<IActionResult> GetEmergencyServiceByType(string serviceType)
        {
            var response = await _emergencyServiceService.GetEmergencyServiceByServiceType(
                serviceType
            );
            return Ok(response);
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
