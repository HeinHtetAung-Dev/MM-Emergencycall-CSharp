using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MMEmergencyCall.Domain.Client.Middlewares;
using MMEmergencyCall.Shared;

namespace MMEmergencyCall.Domain.Client.Features.EmergencyServices
{
    [UserAuthorizeAttribute]
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
        public async Task<IActionResult> GetAllByPaginationAsync(int pageNo, int pageSize, string? serviceType)
        {
            var currentUserId = HttpContext.GetCurrentUserId();

            if (!currentUserId.HasValue)
            {
                return Unauthorized("Unauthorized Request");
            }

            var model = await _emergencyServiceService
                .GetEmergencyServices(pageNo, pageSize, serviceType);
            return Execute(model);
        }

        [HttpGet("{serviceId}")]
        public async Task<IActionResult> GetEmergencyServiceById(int serviceId)
        {
            var currentUserId = HttpContext.GetCurrentUserId();

            if (!currentUserId.HasValue)
            {
                return Unauthorized("Unauthorized Request");
            }
            var response = await _emergencyServiceService.GetEmergencyServiceById(serviceId);
            return Execute(response);
        }

        [HttpGet("/api/EmergencyServices/Distance")]
        public async Task<IActionResult> GetEmergencyServiceWithinDistanceAsync(string? TownshipCode, string? EmergencyType, decimal lat, decimal lng, decimal maxDistanceInMile, int pageNo=1 , int PageSize=10)
        {
            var currentUserId = HttpContext.GetCurrentUserId();

            if (!currentUserId.HasValue)
            {
                return Unauthorized("Unauthorized Request");
            }
            var response = await _emergencyServiceService.GetEmergencyServiceWithinDistanceAsync(TownshipCode, EmergencyType, lat, lng, maxDistanceInMile, pageNo, PageSize);
            return Execute(response);
        }

        [HttpPost]
        [UserAuthorizeAttribute]
        public async Task<IActionResult> CreateEmergencyServiceAsync(
            EmergencyServiceRequestModel requestModel
        )
        {
            var currentUserId = HttpContext.GetCurrentUserId();

            if (!currentUserId.HasValue)
            {
                return Unauthorized("Unauthorized Request");
            }
            var model = await _emergencyServiceService.CreateEmergencyServiceAsync(requestModel);
            return Execute(model);
        }

        [HttpPut("{id}")]
        [UserAuthorizeAttribute]
        public async Task<IActionResult> UpdateEmergencyService(int id,
            [FromBody] EmergencyServiceRequestModel requestModel)
        {
            var currentUserId = HttpContext.GetCurrentUserId();
            if (!currentUserId.HasValue)
            {
                return Unauthorized("Unauthorized Request");
            }

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
            var currentUserId = HttpContext.GetCurrentUserId();

            if (!currentUserId.HasValue)
            {
                return Unauthorized("Unauthorized Request");
            }

            var model = await _emergencyServiceService.DeleteEmergencyService(id);
            return Execute(model);
        }

    }
}
