using Microsoft.Extensions.Logging;
using System.Net.Mail;
using System.Net;
using MMEmergencyCall.Databases.AppDbContextModels;
using System.Data;
using static System.Net.WebRequestMethods;

namespace MMEmergencyCall.Domain.Client.Features.Register;

public class RegisterService
{
    private readonly ILogger<RegisterService> _logger;
    private readonly AppDbContext _db;

    public RegisterService(ILogger<RegisterService> logger, AppDbContext context)
    {
        _logger = logger;
        _db = context;
    }

    private void SendEmail(string toEmail, string otp)
    {
        using (var mail = new MailMessage())
        {
            mail.From = new MailAddress("mmemergencycall@gmail.com");
            mail.To.Add(toEmail);
            mail.Subject = "Email Verification OTP";
            mail.Body = $"Your OTP is: {otp}";
            mail.IsBodyHtml = false;

            using (var smtp = new SmtpClient("smtp.gmail.com", 587))
            {
                smtp.Credentials = new NetworkCredential("mmemergencycall@gmail.com", "xzrl ajmh aumn ixmw");
                smtp.EnableSsl = true;
                smtp.Send(mail);
            }
        }
    }

    public async Task<Result<RegisterModel>> RegisterUserAsync(RegisterRequestModel request)
    {
        try
        {
            User user = new User();
            var otp = new Random().Next(100000, 999999).ToString();

            if (!request.Email.Contains("@gmail.com"))
            {
                return Result<RegisterModel>.ValidationError("Please use correct email format.");
            }
            var CheckExistUser = await _db.Users.Where(x => x.Email == request.Email 
                                        && x.IsVerified == EnumVerify.Y.ToString())
                                        .FirstOrDefaultAsync();

            var CheckVerifyingUser = await _db.Users.Where(x => x.Email == request.Email 
                                            && x.IsVerified == EnumVerify.N.ToString())
                                          .FirstOrDefaultAsync();

            if (CheckExistUser != null)
            {
                return Result<RegisterModel>.ValidationError("This email is already registered. Please use another email.");
            }
            else if (CheckVerifyingUser != null)
            {
                user = new User
                {
                    UserId = CheckVerifyingUser.UserId,
                    Name = request.Name,
                    Email = request.Email,
                    PhoneNumber = request.PhoneNumber,
                    Password = request.Password,
                    Address = request.Address,
                    TownshipCode = request.TownshipCode,
                    EmergencyType = request.EmergencyType,
                    EmergencyDetails = request.EmergencyDetails,
                    UserStatus = EnumUserStatus.Pending.ToString(),
                    Role = "Normal User",
                    Otp = otp,
                    IsVerified = EnumVerify.N.ToString()
                };
                _db.Users.Update(user);
            }
            else
            {
                user = new User
                {
                    Name = request.Name,
                    Email = request.Email,
                    PhoneNumber = request.PhoneNumber,
                    Password = request.Password,
                    Address = request.Address,
                    TownshipCode = request.TownshipCode,
                    EmergencyType = request.EmergencyType,
                    EmergencyDetails = request.EmergencyDetails,
                    UserStatus = EnumUserStatus.Pending.ToString(),
                    Role = "Normal User",
                    Otp = otp,
                    IsVerified = EnumVerify.N.ToString()
                };
                _db.Users.Add(user);
            }
            await _db.SaveChangesAsync();
            SendEmail(request.Email, otp);
            var model = new RegisterModel()
            {
                UserId = user.UserId,
                Name = request.Name,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
                Address = request.Address,
                TownshipCode = request.TownshipCode,
                EmergencyType = request.EmergencyType,
                EmergencyDetails = request.EmergencyDetails,
                Role = user.Role,
                UserStatus = user.UserStatus,
                IsVerified = user.IsVerified,
                OTP = user.Otp
            };
            return Result<RegisterModel>.Success(model, "Registration successful. Check your email for the OTP.");
        }
        catch (Exception ex)
        {
            string message = "An error occurred while registering the user: " + ex.ToString();
            _logger.LogError(message);
            return Result<RegisterModel>.Failure(message);
        }
    }

    public async Task<Result<RegisterModel>> VerifyUserAsync(VerifyRequestModel request)
    {
        try
        {
            var User = await _db.Users.Where(x => x.Email == request.Email 
                            && x.IsVerified == EnumVerify.N.ToString())
                            .FirstOrDefaultAsync();

            if (User != null)
            {
                if (User.Email == request.Email && User.Otp == request.OTP)
                {
                    User.IsVerified = EnumVerify.Y.ToString();
                    User.UserStatus = EnumUserStatus.Approved.ToString();
                    _db.Users.Update(User);
                    await _db.SaveChangesAsync();
                }
                else
                {
                    return Result<RegisterModel>.ValidationError("Please use the correct email and OTP.");
                }
            }
            else
            {
                return Result<RegisterModel>.ValidationError("Please use the correct email.");
            }

            var model = new RegisterModel()
            {
                UserId = User.UserId,
                Name = User.Name,
                Email = User.Email,
                PhoneNumber = User.PhoneNumber,
                Address = User.Address,
                TownshipCode = User.TownshipCode,
                EmergencyType = User.EmergencyType,
                EmergencyDetails = User.EmergencyDetails,
                Role = User.Role,
                UserStatus = User.UserStatus,
                IsVerified = User.IsVerified,
                OTP = User.Otp
            };
            return Result<RegisterModel>.Success(model, "Verification successful.");
        }
        catch (Exception ex)
        {
            string message = "An error occurred while registering the user: " + ex.ToString();
            _logger.LogError(message);
            return Result<RegisterModel>.Failure(message);
        }
    }
}
