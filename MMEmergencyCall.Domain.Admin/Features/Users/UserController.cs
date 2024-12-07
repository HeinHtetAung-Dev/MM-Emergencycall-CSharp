namespace MMEmergencyCall.Domain.Admin.Features.Users;

[Route("api/[controller]")]
[AdminAuthorize]
[ApiController]
public class UserController : BaseController
{
    private readonly UserService _userService;

    public UserController(UserService userService)
    {
        _userService = userService;
    }

    [HttpGet("pageNo/{pageNo}/pageSize/{pageSize}")]
    public async Task<IActionResult> GetUsersByRoleAsync(string? role, string? userStatus, int pageNo = 1, int pageSize = 10)
    {
        var model = await _userService.GetUsersAsync(pageNo, pageSize, role, userStatus);
        return Execute(model);
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

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUserAsync(int id, UserRequestModel requestModel)
    {
        var model = await _userService.UpdateUserAsync(id, requestModel);
        return Execute(model);
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> UpdateUserStatusAsync(int id, UserStatusRequestModel statusRequest)
    {
        var model = await _userService.UpdateUserStatusAsync(id, statusRequest);
        return Execute(model);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUserAsync(int id)
    {
        var model = await _userService.DeleteUserAsync(id);
        return Execute(model);
    }
}
