using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MMEmergencyCall.Domain.User.Features.EmergencyServiceType;

namespace MMEmergencyCall.Api.Endpoints.EmergencyService
{
    [Route("api/[controller]")]
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
        public IActionResult GetEmergencyServiceTypes()
        {
            var response = _emergencyServiceTypeService.GetServiceTypes();
            return Ok(response.Result);
        }
    }
}
