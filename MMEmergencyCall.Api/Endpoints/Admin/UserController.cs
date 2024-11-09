using Microsoft.AspNetCore.Mvc;
using MMEmergencyCall.Domain.Admin.Features.Users;

namespace MMEmergencyCall.Api.Endpoints.Admin;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly UserService _userService;

    public UserController(UserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var model = await _userService.GetAllAsync();
        if (!model.IsSuccess)
            return NotFound(model);

        return Ok(model);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetByIdAsync(int id)
    {
        var model = await _userService.GetByIdAsync(id);
        if(!model.IsSuccess)
            return NotFound(model);

        return Ok(model);
    }
}
