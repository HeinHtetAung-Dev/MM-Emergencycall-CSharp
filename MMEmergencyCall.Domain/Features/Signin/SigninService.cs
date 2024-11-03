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

    public SigninResponseModel Signin(SigninRequestModel requestModel)
    {        
        var user = _db.Users
            .Where(u => u.Name == requestModel.Name
                    && u.PhoneNumber == requestModel.PhoneNumber)
            .FirstOrDefault();

        if (user is null)
        {
            return new SigninResponseModel(Result<User>.Failure("Username or Password is incorrect."));
        }      

        return new SigninResponseModel(Result<User>.Success(user));
    } 
}
