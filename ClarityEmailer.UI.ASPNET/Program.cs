using ClarityEmailer.Core.Processors;
using Library.NET.Logging;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

if (!File.Exists("logfile.log"))
{
    File.Create("logfile.log");
}

ICustomLogger logger = new CustomLogger(new("logfile.log"), true, Library.NET.Logging.LogLevel.Information);

builder.Services.AddSingleton(logger);
builder.Services.AddScoped<IEmailProcessor, EmailProcessor>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
