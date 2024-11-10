using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MMEmergencyCall.Domain.Client.Features.EmergencyServices;
using MMEmergencyCall.Shared;

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

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEmergencyService(
            int id,
            [FromBody] EmergencyServiceRequestModel requestModel
        )
        {
            Result<EmergencyServiceResponseModel> model = null;

            if (string.IsNullOrEmpty(requestModel.ServiceType))
            {
                model = Result<EmergencyServiceResponseModel>.Failure("Service Type is required.");
                goto BadRequest;
            }

            if (string.IsNullOrEmpty(requestModel.ServiceGroup))
            {
                model = Result<EmergencyServiceResponseModel>.Failure("Service Group is required.");
                goto BadRequest;
            }

            if (string.IsNullOrEmpty(requestModel.ServiceName))
            {
                model = Result<EmergencyServiceResponseModel>.Failure("Service Name is required.");
                goto BadRequest;
            }

            if (string.IsNullOrEmpty(requestModel.PhoneNumber))
            {
                model = Result<EmergencyServiceResponseModel>.Failure("Phone Number is required.");
                goto BadRequest;
            }

            model = await _emergencyServiceService.UpdateEmergencyService(id, requestModel);
            if (!model.IsSuccess)
                return NotFound(model);

            return Ok(model);

            BadRequest:
            return BadRequest(model);
        }
    }
}
