using MMEmergencyCall.Domain.Admin.Common;
using MMEmergencyCall.Domain.Admin.Features.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMEmergencyCall.Domain.Admin.Features.CreateUser;

[Route("api/admin/CreateUser")]
[AdminAuthorize]
[ApiController]
public class CreateUserController : BaseController
{
	private readonly CreateUserService _createUserService;

	public CreateUserController(CreateUserService createUserService)
	{
		_createUserService = createUserService;
	}

	[HttpPost]
	public async Task<IActionResult> CreateUserAsync(CreateUserRequestModel requestModel)
	{
		var model = await _createUserService.CreateUserAsync(requestModel);
		return Execute(model);
	}
}
