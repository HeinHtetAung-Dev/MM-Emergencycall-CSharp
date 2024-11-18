using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MMEmergencyCall.Databases.AppDbContextModels;

namespace MMEmergencyCall.Domain.Client.Features.Signin;

public class SigninService
{
    private readonly ILogger<User> _logger;

    private readonly AppDbContext _db;

    public SigninService(ILogger<User> logger, AppDbContext db)
    {
        _logger = logger;
        _db = db;
    }

    public async Task<Result<SigninModel>> SigninAsync(SigninRequestModel requestModel)
    {
        var user = await _db.Users.Where(u => u.Email == requestModel.Email
                   && u.Password == requestModel.Password)
                  .FirstOrDefaultAsync();

        if (user is null)
        {
            return Result<SigninModel>.ValidationError("Username or Password is incorrect.");
        }

        SigninModel signin = new()
        {
            Email = user.Email,
            Name = user.Name,
            SessionExpiredTime = DateTime.Now.AddMinutes(5),
            UserId = user.UserId
        };

        var token = signin.ToJson().ToEncrypt();
        signin.Token = token;

        return Result<SigninModel>.Success(signin);
    }
}
