using JobFinder.Application;
using JobFinder.Application.JobScoring;
using JobFinder.Infrastructure;
using JobFinder.Infrastructure.Data;
using JobFinder.Infrastructure.JobScoring;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<JobFinderDbContext>(options =>
              options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

builder.Services.AddControllers();
builder.Services.AddApplicationApi();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddHttpClient<IJobScoringClient, JobScoringClient>(client =>
{
    client.BaseAddress = new Uri("http://localhost:11434");
    client.Timeout = TimeSpan.FromSeconds(120);
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.MapControllers();
app.Run();