using Microsoft.AspNetCore.Mvc;
using MMEmergencyCall.Domain.User.Features.Signin;

namespace MMEmergencyCall.Api.Endpoints.Signin;

[Route("api/[controller]")]
[ApiController
    ]
public class SigninController : ControllerBase
{
    private readonly SigninService _signinService;

    public SigninController(SigninService signinService)
    {
        _signinService = signinService;
    }

    [HttpPost]
    public async Task<IActionResult> SigninAsync(SigninRequestModel requestModel)
    {
        var response = await _signinService.SigninAsync(requestModel);
        return Ok(response.Result);
    }
}
