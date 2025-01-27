using MMEmergencyCall.Domain.Admin.Features.CreateEmergencyService;
using MMEmergencyCall.Domain.Admin.Features.CreateUser;
using MMEmergencyCall.Domain.Admin.Features.DeleteEmergencyService;
using MMEmergencyCall.Domain.Admin.Features.DeleteEmergencyServiceStatus;
using MMEmergencyCall.Domain.Admin.Features.DeleteUser;
using MMEmergencyCall.Domain.Admin.Features.EmergencyRequest;
using MMEmergencyCall.Domain.Admin.Features.EmergencyService;
using MMEmergencyCall.Domain.Admin.Features.RefreshToken;
using MMEmergencyCall.Domain.Admin.Features.Register;
using MMEmergencyCall.Domain.Admin.Features.Signout;
using MMEmergencyCall.Domain.Admin.Features.StateRegions;
using MMEmergencyCall.Domain.Admin.Features.TotalActive;
using MMEmergencyCall.Domain.Admin.Features.Townships;
using MMEmergencyCall.Domain.Admin.Features.TownshipTopTenRequest;
using MMEmergencyCall.Domain.Admin.Features.TownshipTopTenService;
using MMEmergencyCall.Domain.Admin.Features.UpdateEmergencyRequestStatus;
using MMEmergencyCall.Domain.Admin.Features.UpdateEmergencyService;
using MMEmergencyCall.Domain.Admin.Features.UpdateEmergencyServiceStatus;
using MMEmergencyCall.Domain.Admin.Features.UpdateUser;
using MMEmergencyCall.Domain.Admin.Features.UserById;
using MMEmergencyCall.Domain.Admin.Features.UserList;
using MMEmergencyCall.Domain.Admin.Features.UserTopTenRequest;

namespace MMEmergencyCall.Domain.Admin;

public static class AdminServiceExtension
{
	public static void AddAdminServices(this WebApplicationBuilder builder)
	{
		// Emergency Service Features
		builder.Services.AddScoped<CreateEmergencyServiceService>();
		builder.Services.AddScoped<EmergencyServiceService>();
		builder.Services.AddScoped<UpdateEmergencyServiceStatusService>();
		builder.Services.AddScoped<DeleteEmergencyServiceStatusService>();
		builder.Services.AddScoped<UpdateEmergencyServiceService>();
		builder.Services.AddScoped<DeleteEmergencyServiceService>();

		// Emergency Request Features
		builder.Services.AddScoped<EmergencyRequestService>();
		builder.Services.AddScoped<UpdateEmergencyRequestStatusService>();

		// User(Service Provider) Features
		builder.Services.AddScoped<CreateUserService>();
		builder.Services.AddScoped<UserListService>();
		builder.Services.AddScoped<UserByIdService>();
		builder.Services.AddScoped<UpdateUserService>();
		builder.Services.AddScoped<DeleteUserService>();

		// Auth Features
		builder.Services.AddScoped<AdminRegisterService>();
		builder.Services.AddScoped<AdminSigninService>();
		builder.Services.AddScoped<RefreshTokenService>();
		builder.Services.AddScoped<AdminSignoutService>();

		// Code Setup
		builder.Services.AddScoped<StateRegionService>();
		builder.Services.AddScoped<TownshipService>();
		
		// Dashboard Features
		builder.Services.AddScoped<UserTopTenRequestService>();
		builder.Services.AddScoped<TownshipTopTenRequestService>();
		builder.Services.AddScoped<TownshipTopTenServiceService>();
		builder.Services.AddScoped<TotalActiveService>();

	}
}
