using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MMEmergencyCall.Domain.Client.Features.EmergencyServiceType;

namespace MMEmergencyCall.Api.Endpoints.EmergencyService
{
    [Route("api/EmergencyService/ServiceType")]
    [ApiController]
    public class EmergencyServiceTypeController : ControllerBase
    {
        private readonly EmergencyServiceTypeService _emergencyServiceTypeService;

        public EmergencyServiceTypeController(
            EmergencyServiceTypeService emergencyServiceTypeService
        )
        {
            _emergencyServiceTypeService = emergencyServiceTypeService;
        }

        [HttpGet]
        public async Task<IActionResult> GetEmergencyServiceTypesAsync()
        {
            var response = await _emergencyServiceTypeService.GetServiceTypesAsync();
            return Ok(response);
        }
    }
}
