namespace MMEmergencyCall.Domain.Client;

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

        if (model.IsError)
            return InternalServerError(model);

        return Ok(model);
    }
}
