using JobFinder.Application;
using JobFinder.Infrastructure;
using JobFinder.Infrastructure.Data;
using JobFinder.WorkerService.Configuration;
using JobFinder.WorkerService.Worker;
using Microsoft.EntityFrameworkCore;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.Configure<SearchOptions>(
    builder.Configuration.GetSection("SearchOptions")
);

builder.Services.AddDbContext<JobFinderDbContext>(options =>
              options.UseSqlServer(builder.Configuration.GetConnectionString("Default"), b => b.MigrationsAssembly("JobFinder.WorkerService")));

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddHostedService<JobSearchWorker>();

var host = builder.Build();
host.Run();
