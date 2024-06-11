using MessagesExchange.Infrastructure.Database;
using MessagesExchange.Infrastructure.Database.Messages;
using MessagesExchange.Infrastructure.Database.Migrator;
using MessagesExchange.Infrastructure.Database.Migrator.Migrations;
using MessagesExchange.Infrastructure.SignalR;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<SqlConnectionsFactory>();
builder.Services.AddSingleton<IMigrationsRepository, MigrationsRepository>();
builder.Services.AddSingleton<IMigrationsService, MigrationsService>();

builder.Services.AddSingleton<Migration, InitialMigration>();
builder.Services.AddSingleton<Migration, MessagesTableMigration>();

builder.Services.AddScoped<IMessagesRepository, MessagesRepository>();

builder.Services.AddHostedService<Migrator>();

builder.Services.AddSignalR().AddHubOptions<MessagesRealTimeHub>(options =>
{
    options.EnableDetailedErrors = true;
});

builder.Services.AddControllersWithViews()
    .AddRazorRuntimeCompilation();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
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
