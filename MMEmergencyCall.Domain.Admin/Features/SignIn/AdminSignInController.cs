namespace MMEmergencyCall.Domain.Admin.Features.SignIn;


    [Route("api/admin/[controller]")]
    [ApiController]
    public class AdminSigninController : ControllerBase
    {
        private readonly AdminSigninService _signinService;

        public AdminSigninController(AdminSigninService signinService)
        {
            _signinService = signinService;
        }

        [HttpPost]
        public async Task<IActionResult> SigninAsync(AdminSigninRequestModel requestModel)
        {
            var response = await _signinService.SigninAsync(requestModel);
            return Ok(response.Result);
        }
    }
