using Microsoft.AspNetCore.Mvc;
using MMEmergencyCall.Shared;

namespace MMEmergencyCall.Domain.Admin.Features.Townships;

[Route("api/[controller]")]
[ApiController]
public class TownshipController : BaseController
{
    private readonly TownshipService _townshipService;

    public TownshipController(TownshipService townshipService)
    {
        _townshipService = townshipService;
    }

    [HttpGet("pageNo/{pageNo}/pageSize/{pageSize}")]
    public async Task<IActionResult> GetAll(int pageNo, int pageSize)
    {
        var model = await _townshipService.GetAllAsync(pageNo, pageSize);
        return Execute(model);
    }
}