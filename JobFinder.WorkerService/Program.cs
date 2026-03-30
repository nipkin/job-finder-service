using JobFinder.Application;
using JobFinder.Application.JobScoring;
using JobFinder.Infrastructure;
using JobFinder.Infrastructure.Data;
using JobFinder.Infrastructure.JobScoring;
using JobFinder.WorkerService.Configuration;
using JobFinder.WorkerService.Worker;
using Microsoft.EntityFrameworkCore;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.Configure<SearchOptions>(
    builder.Configuration.GetSection("SearchOptions")
);
builder.Services.Configure<JobSkills>(
    builder.Configuration.GetSection("JobSkills")
);

builder.Services.AddDbContext<JobFinderDbContext>(options =>
              options.UseSqlServer(builder.Configuration.GetConnectionString("Default"), 
              b => b.MigrationsAssembly("JobFinder.WorkerService")));

builder.Services.AddApplication();
builder.Services.AddInfrastructure();
builder.Services.AddHttpClient<IJobScoringClient, JobScoringClient>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["OllamaBaseUrl"]!);
    client.Timeout = TimeSpan.FromSeconds(120);
});

builder.Services.AddHostedService<JobSearchWorker>();

var host = builder.Build();
host.Run();
