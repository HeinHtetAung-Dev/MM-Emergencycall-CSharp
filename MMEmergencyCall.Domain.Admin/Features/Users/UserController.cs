using Microsoft.AspNetCore.Mvc;
using MMEmergencyCall.Domain.Admin.Middlewares;

namespace MMEmergencyCall.Domain.Admin.Features.Users;

[Route("api/[controller]")]
[AdminAuthorizeAttribute]
[ApiController]
public class UserController : BaseController
{
    private readonly UserService _userService;

    public UserController(UserService userService)
    {
        _userService = userService;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetByIdAsync(int id)
    {
        var model = await _userService.GetByIdAsync(id);
        return Execute(model);
    }


    [HttpPost]
    public async Task<IActionResult> CreateUserAsync(UserRequestModel requestModel)
    {
        var model = await _userService.CreateUserAsync(requestModel);
        return Execute(model);
    }

    [HttpGet("pageNo/{pageNo}/pageSize/{pageSize}")]
    public async Task<IActionResult> GetUsersByRoleAsync(string? role, int pageNo = 1, int pageSize = 10)
    {
        var model = await _userService.GetUsersByRoleAsync(pageNo, pageSize, role);
        return Execute(model);
    }

    [HttpGet("userStatus/{userStatus}/pageNo/{pageNo}/pageSize/{pageSize}")]
    public async Task<IActionResult> GetUsersByUserStatusAsync(string userStatus, int pageNo = 1, int pageSize = 10)
    {
        var model = await _userService.GetUsersByUserStatusAsync(userStatus, pageNo, pageSize);
        return Execute(model);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUserAsync(int id, UserRequestModel requestModel)
    {
        var model = await _userService.UpdateUserAsync(id, requestModel);
        return Execute(model);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUserAsync(int id)
    {
        var model = await _userService.DeleteUserAsync(id);
        return Execute(model);
    }
}
