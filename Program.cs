using MessagesExchange.Data.Messages;
using MessagesExchange.Infrastructure;
using MessagesExchange.Infrastructure.Migrations;
using MessagesExchange.Infrastructure.Migrations.DatabaseMigrations;
using MessagesExchange.Infrastructure.Migrator;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton<SqlConnectionsFactory>();
builder.Services.AddSingleton<IMigrationsRepository, MigrationsRepository>();
builder.Services.AddSingleton<IMigrationsService, MigrationsService>();

builder.Services.AddSingleton<Migration, InitialMigration>();
builder.Services.AddSingleton<Migration, MessagesTableMigration>();

builder.Services.AddScoped<IMessagesRepository, MessagesRepository>();

builder.Services.AddHostedService<Migrator>();

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
