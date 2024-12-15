namespace MMEmergencyCall.Domain.Admin.Features;

public class BaseController : ControllerBase
{
    protected IActionResult InternalServerError(object? obj = null)
    {
        return StatusCode(500, obj);
    }

    public IActionResult Execute<T>(Result<T> model)
    {
        if (model.IsValidationError)
            return BadRequest(model);

        if (model.IsNotFoundError)
            return NotFound(model);

        if (model.IsError)
            return InternalServerError(model);

        return Ok(model);
    }
}
