using JobFinder.Api.Configuration;
using JobFinder.Api.Features.Auth;
using JobFinder.Application;
using JobFinder.Application.JobScoring;
using JobFinder.Infrastructure;
using JobFinder.Infrastructure.Data;
using JobFinder.Infrastructure.JobScoring;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<JobFinderDbContext>(options =>
              options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

builder.Services.Configure<JobMatchOptions>(builder.Configuration.GetSection("JobMatch"));
builder.Services.AddControllers();
builder.Services.AddApplicationApi();
builder.Services.AddInfrastructure();
builder.Services.AddScoped<ITokenService, TokenService>();

builder.Services.AddHttpClient<IJobScoringClient, JobScoringClient>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["OllamaBaseUrl"]!);
    client.Timeout = TimeSpan.FromSeconds(120);
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
        };

        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                context.Token = context.Request.Cookies["auth_token"];
                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddAuthorization();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
