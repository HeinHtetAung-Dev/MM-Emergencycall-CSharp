using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MMEmergencyCall.Domain.Features.Register;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMEmergencyCall.Domain.Features.Signin;

public class SigninService
{
    private readonly ILogger<User> _logger;

    private readonly AppDbContext _db;

    public SigninService(ILogger<User> logger, AppDbContext db)
    {
        _logger = logger;
        _db = db;
    }

    public async Task<SigninResponseModel> SigninAsync(SigninRequestModel requestModel)
    {        
        var user = await _db.Users.AsNoTracking()
            .Where(u => u.Email == requestModel.Email
                    && u.Password == requestModel.Password)
            .FirstOrDefaultAsync();

        if (user is null)
        {
            return new SigninResponseModel(Result<SigninModel>.Failure("Username or Password is incorrect."));
        }

        SigninModel signin = new SigninModel
        {
            Email = user.Email,
            Name = user.Name,
            SessionExpiredTime = DateTime.Now.AddMinutes(5),
            UserId = user.UserId
        };

        var token = signin.ToJson().ToEncrypt();
        signin.Token = token;

        return new SigninResponseModel(Result<SigninModel>.Success(signin));
    } 
}
