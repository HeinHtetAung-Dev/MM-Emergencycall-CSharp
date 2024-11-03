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
            return new SigninResponseModel(Result<User>.Failure("Username or Password is incorrect."));
        }      

        return new SigninResponseModel(Result<User>.Success(user));
    } 
}
