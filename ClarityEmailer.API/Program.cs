using ClarityEmailer.API.Middleware;
using ClarityEmailer.Core.Processors;

using Library.NET.Logging;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

ICustomLogger logger = new CustomLogger(new("EmailProcessor.log"), true, Library.NET.Logging.LogLevel.Information);

builder.Services.AddSingleton(logger);
builder.Services.AddScoped<IEmailProcessor, EmailProcessor>();

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseMiddleware<ApiKeyMiddleware>();

app.Run();
