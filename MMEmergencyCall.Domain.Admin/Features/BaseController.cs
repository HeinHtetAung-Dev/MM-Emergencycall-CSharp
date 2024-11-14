using MMEmergencyCall.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMEmergencyCall.Domain.Admin.Features
{
    public class BaseController : ControllerBase
    {
        public IActionResult InternalServerError(object? obj = null)
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
}
