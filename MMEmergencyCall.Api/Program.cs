using Microsoft.EntityFrameworkCore;
using MMEmergencyCall.Databases.AppDbContextModels;
using MMEmergencyCall.Domain.Admin.Features.StateRegion;
using MMEmergencyCall.Domain.Admin.Features.Users;
using MMEmergencyCall.Domain.Client.Features.EmergencyRequests;
using MMEmergencyCall.Domain.Client.Features.EmergencyServices;
using MMEmergencyCall.Domain.Client.Features.EmergencyServiceType;
using MMEmergencyCall.Domain.Client.Features.Register;
using MMEmergencyCall.Domain.Client.Features.Signin;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddHttpContextAccessor();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(opt =>
{
    opt.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DbConnection"));
});

builder.AddRegisterService();
builder.AddEmergencyServiceService();
builder.AddEmergencyServiceType();
builder.AddEmergencyRequest();
builder.AddSigninService();
builder.AddStateRegionService();
builder.AddUserService();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
