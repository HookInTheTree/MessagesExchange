using MessagesExchange.Infrastructure.Database;
using MessagesExchange.Infrastructure.Database.Messages;
using MessagesExchange.Infrastructure.Database.Migrator;
using MessagesExchange.Infrastructure.Database.Migrator.Migrations;
using MessagesExchange.Infrastructure.Logging;
using MessagesExchange.Infrastructure.SignalR;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<SqlConnectionsFactory>();
builder.Services.AddSingleton<IMigrationsRepository, MigrationsRepository>();
builder.Services.AddSingleton<IMigrationsService, MigrationsService>();

builder.Services.AddSingleton<Migration, InitialMigration>();
builder.Services.AddSingleton<Migration, MessagesTableMigration>();

builder.Services.AddScoped<IMessagesRepository, MessagesRepository>();

builder.Services.AddHostedService<Migrator>();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Messages Exchange WEB API",
        Description = "Web API documentation for Message Exchange app",
    });

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    options.IncludeXmlComments(xmlPath);
});

builder.Services.AddSignalR()
    .AddHubOptions<MessagesRealTimeHub>(options =>
    {
        options.EnableDetailedErrors = true;
    });

builder.Services.AddControllersWithViews()
    .AddRazorRuntimeCompilation();

builder.Services.AddLogging(loggerBuilder =>
{
    var logger = new LoggerConfiguration()
                .ReadFrom
                .Configuration(builder.Configuration)
                .CreateLogger();

    loggerBuilder.AddSerilog(logger);
});

builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<RequestsExceptionsHandler>();

var app = builder.Build();

app.UseExceptionHandler();

if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
}
else
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Clients}/{action=Reader}/{id?}");

app.MapHub<MessagesRealTimeHub>("/real-time-messages");



app.Run();
