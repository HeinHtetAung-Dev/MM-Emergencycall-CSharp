using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using MMEmergencyCall.Shared;
using MMEmergencyCall.Domain.Client.Features.Signin;

namespace MMEmergencyCall.Api.Middlewares
{
    public class CustomAuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (!context.HttpContext!.Request.Headers["Token"].Any())
            {
                context.Result = new UnauthorizedObjectResult(string.Empty);
                return;
            }

            // ToJson => ToEncrypt
            // ToDecrypt => ToObject

            try
            {
                var signin = context.HttpContext!.Request.Headers["Token"]
                    .ToString()
                    .ToDecrypt()
                    .ToObject<SigninModel>();

                // if (signin.UserId) // if exist in db?
                //if(signin.SessionExpiredTime > DateTime.Now)
            }
            catch (Exception ex)
            {
                context.Result = new UnauthorizedObjectResult(string.Empty);
                return;
            }
        }
    }
}
