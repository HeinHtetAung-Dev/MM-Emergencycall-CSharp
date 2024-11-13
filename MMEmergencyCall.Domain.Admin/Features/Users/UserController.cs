using Microsoft.AspNetCore.Mvc;

namespace MMEmergencyCall.Domain.Admin.Features.Users;

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
}
