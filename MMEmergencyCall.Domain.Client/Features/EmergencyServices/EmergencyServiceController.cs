using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MMEmergencyCall.Domain.Client.Middlewares;
using MMEmergencyCall.Shared;

namespace MMEmergencyCall.Domain.Client.Features.EmergencyServices
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmergencyServicesController : BaseController
    {
        private readonly EmergencyServiceService _emergencyServiceService;

        public EmergencyServicesController(EmergencyServiceService emergencyServiceService)
        {
            _emergencyServiceService = emergencyServiceService;
        }

        [HttpGet("pageNo/{pageNo}/pageSize/{pageSize}")]
        public async Task<IActionResult> GetAllByPaginationAsync(string? serviceType,int pageNo = 1, int pageSize = 10)
        {
            var model = await _emergencyServiceService
                .GetEmergencyServices(serviceType, pageNo, pageSize);
            return Execute(model);
        }

        [HttpGet("{serviceId}")]
        public async Task<IActionResult> GetEmergencyServiceById(int serviceId)
        {
            var response = await _emergencyServiceService.GetEmergencyServiceById(serviceId);
            return Execute(response);
        }

        //[HttpGet("ServiceType/{serviceType}")]
        //public async Task<IActionResult> GetServiceByServiceTypeWithPagination(
        //    string serviceType,
        //    int pageNo,
        //    int pageSize
        //)
        //{
        //    var response = await _emergencyServiceService.GetServiceByServiceTypeWithPagination(
        //        serviceType,
        //        pageNo,
        //        pageSize
        //    );
        //    return Execute(response);
        //}

        [HttpPost]
        [UserAuthorizeAttribute]
        public async Task<IActionResult> CreateEmergencyServiceAsync(
            EmergencyServiceRequestModel requestModel
        )
        {
            var model = await _emergencyServiceService.CreateEmergencyServiceAsync(requestModel);
            return Execute(model);
        }

        [HttpPut("{id}")]
        [UserAuthorizeAttribute]
        public async Task<IActionResult> UpdateEmergencyService(int id,
            [FromBody] EmergencyServiceRequestModel requestModel)
        {
            Result<EmergencyServiceResponseModel> model = null;

            if (string.IsNullOrEmpty(requestModel.ServiceType))
            {
                model = Result<EmergencyServiceResponseModel>.ValidationError(
                    "Service Type is required."
                );
                goto BadRequest;
            }

            if (string.IsNullOrEmpty(requestModel.ServiceGroup))
            {
                model = Result<EmergencyServiceResponseModel>.ValidationError(
                    "Service Group is required."
                );
                goto BadRequest;
            }

            if (string.IsNullOrEmpty(requestModel.ServiceName))
            {
                model = Result<EmergencyServiceResponseModel>.ValidationError(
                    "Service Name is required."
                );
                goto BadRequest;
            }

            if (string.IsNullOrEmpty(requestModel.PhoneNumber))
            {
                model = Result<EmergencyServiceResponseModel>.ValidationError(
                    "Phone Number is required."
                );
                goto BadRequest;
            }

            model = await _emergencyServiceService.UpdateEmergencyService(id, requestModel);

            return Execute(model);

        BadRequest:
            return BadRequest(model);
        }

        [HttpDelete("{id}")]
        [UserAuthorizeAttribute]
        public async Task<IActionResult> DeleteEmergencyService(int id)
        {
            var model = await _emergencyServiceService.DeleteEmergencyService(id);
            return Execute(model);
        }
    }
}
