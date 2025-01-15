using System.Reflection;
using System.Text.Json;
using AdminService.Application;
using AdminService.Infrastructure;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

using var loggerFactory = LoggerFactory.Create(x =>
{
    x.AddJsonConsole(options =>
    {
        options.IncludeScopes = false;
        options.TimestampFormat = "HH:mm:ss ";
        options.JsonWriterOptions = new JsonWriterOptions
        {
            Indented = true
        };
    });
    x.SetMinimumLevel(LogLevel.Debug);
});

loggerFactory.CreateLogger<Program>();


builder.Services.AddRouting(options => options.LowercaseUrls = true);
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Internal Admin Service", 
        Description = "Internal Admin Service API used to manage sensitive words",
        Version = "v1"
    });
    
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    
    c.IncludeXmlComments(xmlPath);
});

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

if (string.IsNullOrWhiteSpace(connectionString)) throw new ApplicationException("No connection string found.");

builder
    .Services
    .AddApplication()
    .AddInfrastructure(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();
