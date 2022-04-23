using ClarityEmailer.API.Middleware;
using ClarityEmailer.Core.Processors;
using ClarityEmailer.Core;

using Library.NET.Logging;
using LogLevel = Library.NET.Logging.LogLevel;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

if (!File.Exists("logfile.log"))
{
    File.Create("logfile.log");
}

ICustomLogger logger = new CustomLogger(new("logfile.log"), true, LogLevel.Information);

builder.Services.AddSingleton(logger);
builder.Services.AddScoped<IEmailProcessor, EmailProcessor>();

var app = builder.Build();

var appSettingsFile = "appSettings.json";

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    appSettingsFile = "appSettings.Development.json";
}

IConfigurationRoot configuration = new ConfigurationBuilder()
    .AddJsonFile(appSettingsFile, false, true)
    .Build();

GlobalConfig.EmailConfig = new()
{
    SmtpServer = configuration["Email Settings:Smtp Server"],
    SmtpPort = Convert.ToInt16(configuration["Email Settings:Smtp Port"]),
    SenderEmail = configuration["Email Settings:Sender Email"],
    Password = configuration["Email Settings:Password"],
};

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseMiddleware<ApiKeyMiddleware>();

app.Run();
