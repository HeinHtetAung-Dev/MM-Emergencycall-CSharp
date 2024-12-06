﻿using Microsoft.Extensions.Logging;
using System.Net.Mail;
using System.Net;

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
            var checkExistUser = await _db.Users.Where(x => x.Email == request.Email 
                                        && x.IsVerified == EnumVerify.Y.ToString())
                                        .FirstOrDefaultAsync();

            var checkVerifyingUser = await _db.Users.Where(x => x.Email == request.Email 
                                            && x.IsVerified == EnumVerify.N.ToString())
                                          .FirstOrDefaultAsync();

            if (checkExistUser != null)
            {
                return Result<RegisterModel>.ValidationError("This email is already registered. Please use another email.");
            }
            else if (checkVerifyingUser != null)
            {
                user = new User
                {
                    UserId = checkVerifyingUser.UserId,
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
                Otp = user.Otp
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
            var user = await _db.Users.Where(x => x.Email == request.Email 
                            && x.IsVerified == EnumVerify.N.ToString())
                            .FirstOrDefaultAsync();

            if (user != null)
            {
                if (user.Email == request.Email && user.Otp == request.Otp)
                {
                    user.IsVerified = EnumVerify.Y.ToString();
                    user.UserStatus = EnumUserStatus.Approved.ToString();
                    _db.Users.Update(user);
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
                UserId = user.UserId,
                Name = user.Name,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Address = user.Address,
                TownshipCode = user.TownshipCode,
                EmergencyType = user.EmergencyType,
                EmergencyDetails = user.EmergencyDetails,
                Role = user.Role,
                UserStatus = user.UserStatus,
                IsVerified = user.IsVerified,
                Otp = user.Otp
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
