using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MMEmergencyCall.Shared;

namespace MMEmergencyCall.Domain.Client.Features.EmergencyServices
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmergencyServiceController : BaseController
    {
        private readonly EmergencyServiceService _emergencyServiceService;

        public EmergencyServiceController(EmergencyServiceService emergencyServiceService)
        {
            _emergencyServiceService = emergencyServiceService;
        }

        [HttpGet("pageNo/{pageNo}/pageSize/{pageSize}")]
        public async Task<IActionResult> GetAllByPaginationAsync(int pageNo = 1, int pageSize = 10)
        {
            var model = await _emergencyServiceService
                .GetAllEmergencyServiceWithPagination(pageNo, pageSize);
            return Execute(model);
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
            var model = await _emergencyServiceService
                .GetEmergencyServiceByServiceType(serviceType);
            return Execute(model);
        }

        [HttpPost]
        public async Task<IActionResult> CreateEmergencyServiceAsync(
            EmergencyServiceRequestModel requestModel
        )
        {
            var model = await _emergencyServiceService.CreateEmergencyServiceAsync(requestModel);
            return Execute(model);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEmergencyService(int id,
            [FromBody] EmergencyServiceRequestModel requestModel)
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

            return Execute(model);

        BadRequest:
            return BadRequest(model);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmergencyService(int id)
        {
            var model = await _emergencyServiceService.DeleteEmergencyService(id);
            return Execute(model);
        }
    }
}
