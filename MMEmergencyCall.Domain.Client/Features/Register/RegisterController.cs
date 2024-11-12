namespace MMEmergencyCall.Domain.Client.Features.Register;

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
    public async Task<IActionResult> Register(RegisterRequestModel requestModel)
    {
        return Ok(await _registerService.RegisterUserAsync(requestModel)); 
    }
}