using Application.Interfaces;
using Hangfire;
using Hangfire.PostgreSql;
using Infrastructure.BackgroundTask;
using Infrastructure.Data;
using Infrastructure.Jwt;
using Infrastructure.Seed;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using WebApp.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<JwtOption>(builder.Configuration.GetSection(nameof(JwtOption)));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Authorization
builder.Services.AddRegisterSwaggerGen();
builder.Services.AddHttpContextAccessor();

builder.Services.AddDbContext<ApplicationDbContext>(op =>
    op.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnectionString")));

builder.Services.AddDistributedMemoryCache();
//AddScoped
builder.Services.AddRegistrationServices();

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration["Redis:ConnectionString"];
});

builder.Services.AddJwtRegister(builder.Configuration);
builder.Services.AddAuthorization();

//register hangfire
builder.Services.AddHangfire(config =>
    config.UsePostgreSqlStorage(builder.Configuration.GetConnectionString("DefaultConnectionString")));
builder.Services.AddHangfireServer();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

//seed - created user admin
//Background Task - Hangfire
using (var scope = app.Services.CreateScope())
{
    //seed
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    await context.Database.MigrateAsync();
    var serviceProvider = scope.ServiceProvider;
    await SeedAdmin.Initialize(serviceProvider);
    
    //Background Task
    var recurringJobManager = serviceProvider.GetRequiredService<IRecurringJobManager>();

    recurringJobManager.AddOrUpdate<LowStockTelegramService>(
        "delete-categories",
        service => service.SendMessage(),
        "0 0 1 * *");
}
// open file from chrome
app.UseHangfireDashboard();
app.UseStaticFiles();
app.MapControllers();
app.Run();