using Microsoft.AspNetCore.Mvc;

namespace MMEmergencyCall.Domain.Admin.Features.Users;

[Route("api/[controller]")]
[ApiController]
public class UserController : BaseController
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
        if (!model.IsSuccess)
            return NotFound(model);

        return Ok(model);
    }

    [HttpGet("{pageNo}/{pageSize}")]
    [HttpGet("pageNo/{pageNo}/pageSize/{pageSize}")]
    public async Task<IActionResult> GetAllByPiginationAsync(int pageNo, int pageSize)
    {
        var model = await _userService.GetAllUsersWithPaginationAsync(pageNo, pageSize);
        return Ok(model);
    }

    [HttpPost]
    public async Task<IActionResult> CreateUserAsync(UserRequestModel requestModel)
    {
        var model = await _userService.CreateUserAsync(requestModel);
        return Ok(model);
    }

    [HttpGet("role/{role}/pageNo/{pageNo}/pageSize/{pageSize}")]
    public async Task<IActionResult> GetUsersByRoleAsync(string role, int pageNo, int pageSize)
    {
        var model = await _userService.GetUsersByRoleAsync(role, pageNo, pageSize);
        return Ok(model);
    }

    [HttpGet("userStatus/{userStatus}/pageNo/{pageNo}/pageSize/{pageSize}")]
    public async Task<IActionResult> GetUsersByUserStatusAsync(string userStatus, int pageNo, int pageSize)
    {
        var model = await _userService.GetUsersByUserStatusAsync(userStatus, pageNo, pageSize);
        //if (model.IsValidationError)
        //    return BadRequest(model);

        //if (model.IsError)
        //    return InternalServerError(model);

        //return Ok(model);

        return Execute(model);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUserAsync(int id, UserRequestModel requestModel)
    {
        var model = await _userService.UpdateUserAsync(id, requestModel);
        return Ok(model);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUserAsync(int id)
    {
        var model = await _userService.DeleteUserAsync(id);
        return Ok(model);
    }
}
