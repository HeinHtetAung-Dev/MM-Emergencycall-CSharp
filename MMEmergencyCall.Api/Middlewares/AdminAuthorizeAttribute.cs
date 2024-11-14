using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using MMEmergencyCall.Shared;
using MMEmergencyCall.Domain.Client.Features.Signin;
using MMEmergencyCall.Domain.Admin.Features.Users;

namespace MMEmergencyCall.Api.Middlewares;

public class AdminAuthorizeAttribute : Attribute, IAsyncAuthorizationFilter
{
    private readonly UserService _userService;
    public AdminAuthorizeAttribute(UserService userService)
    {
        _userService = userService;
    }

    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        if (!context.HttpContext!.Request.Headers["Token"].Any())
        {
            context.Result = new UnauthorizedObjectResult(string.Empty);
            return;
        }

        try
        {
            var item = context.HttpContext!.Request.Headers["Token"]
                .ToString()
                .ToDecrypt()
                .ToObject<SigninModel>();

            if (!await IsExistAdmin(item.UserId))
            {
                context.Result = new UnauthorizedObjectResult("Admin does not exist.");
                return;
            }

            if (item.SessionExpiredTime <= DateTime.Now)
            {
                context.Result = new UnauthorizedObjectResult("Session has expired.");
                return;
            }
        }
        catch (Exception)
        {
            context.Result = new UnauthorizedObjectResult("Invalid token.");
            return;
        }
    }

    private async Task<bool> IsExistAdmin(int userId)
    {
        var user = await _userService.IsExistAdmin(userId);
        return user is not null;
    }
}

