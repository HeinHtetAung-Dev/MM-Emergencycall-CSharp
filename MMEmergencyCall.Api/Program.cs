using Microsoft.EntityFrameworkCore;
using MMEmergencyCall.Databases.AppDbContextModels;
using MMEmergencyCall.Domain.Features.EmergencyRequests;
using MMEmergencyCall.Domain.Features.EmergencyServices;
using MMEmergencyCall.Domain.Features.EmergencyServiceType;
using MMEmergencyCall.Domain.Features.Register;
using MMEmergencyCall.Domain.Features.ServiceProvider;
using MMEmergencyCall.Domain.Features.Signin;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

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
builder.AddServiceProviderType();
builder.AddEmergencyRequest();
builder.AddSigninService();

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
