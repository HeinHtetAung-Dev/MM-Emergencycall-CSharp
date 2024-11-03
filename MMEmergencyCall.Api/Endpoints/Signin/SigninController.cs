using Microsoft.AspNetCore.Mvc;
using MMEmergencyCall.Domain.Features.Signin;

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
    public IActionResult Signin(SigninRequestModel requestModel)
    {
        var response = _signinService.Signin(requestModel);
        return Ok(response.Result);
    }
}
