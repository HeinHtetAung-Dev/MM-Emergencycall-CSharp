using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MMEmergencyCall.Domain.Features.Register;

namespace MMEmergencyCall.Api.Endpoints.Register
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegisterController : ControllerBase
    {
        private readonly RegisterService _registerService;

        public RegisterController(RegisterService registerService)
        {
            _registerService = registerService;
        }

        [HttpPost]
        public IActionResult Register(RegisterRequestModel requestModel)
        {
            return Ok(_registerService.Register(requestModel)); 
        }
    }
}
