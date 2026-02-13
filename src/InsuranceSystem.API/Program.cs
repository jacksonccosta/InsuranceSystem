using InsuranceSystem.Infrastructure.Context;
using InsuranceSystem.Infrastructure.Repositories;
using Microsoft.OpenApi.Models;
using System.Reflection;
using InsuranceSystem.Application.Interfaces;
using InsuranceSystem.Application.DTOs;
using Microsoft.EntityFrameworkCore;
using Refit;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IInsuranceRepository, InsuranceRepository>();

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(InsuranceResponse).Assembly));

string externalUrl = builder.Configuration["ExternalServices:InsuredApi"] ?? "http://localhost:3000";
builder.Services.AddRefitClient<IInsuredService>()
    .ConfigureHttpClient(c => c.BaseAddress = new Uri(externalUrl));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "InsuranceSystem API",
        Version = "v1",
        Description = "API de cálculo de seguros de veículos utilizando Clean Architecture e DDD.",
        Contact = new OpenApiContact
        {
            Name = "Jackson Costa",
            Url = new Uri("https://github.com/jacksonccosta/InsuranceSystem")
        }
    });

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        b => b.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var logger = services.GetRequiredService<ILogger<Program>>();
    var context = services.GetRequiredService<ApplicationDbContext>();

    try
    {
        // Retry logic for database connection
        var retryCount = 0;
        var maxRetries = 10;
        var delay = TimeSpan.FromSeconds(5);

        while (retryCount < maxRetries)
        {
            try
            {
                logger.LogInformation($"Attempting database creation/migration (Attempt {retryCount + 1}/{maxRetries})...");
                context.Database.EnsureCreated();
                logger.LogInformation("Database created/verified successfully.");
                break;
            }
            catch (Exception ex)
            {
                logger.LogWarning($"Database not ready yet: {ex.Message}");
            }

            retryCount++;
            if (retryCount == maxRetries)
            {
                logger.LogError("Failed to connect to the database after multiple attempts.");
                throw new Exception("Database unavailable");
            }
            
            Thread.Sleep(delay);
        }
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "An error occurred while migrating the database.");
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");
app.UseAuthorization();
app.MapControllers();

app.Run();