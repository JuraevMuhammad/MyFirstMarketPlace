using Hangfire;
using Hangfire.PostgreSql;
using Infrastructure.BackgroundTask;
using Infrastructure.Data;
using Infrastructure.Email;
using Infrastructure.Jwt;
using Infrastructure.Seed;
using Infrastructure.Telegram;
using Microsoft.EntityFrameworkCore;
using WebApp.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<JwtOption>(builder.Configuration.GetSection(nameof(JwtOption)));
builder.Services.Configure<EmailOptions>(builder.Configuration.GetSection(nameof(EmailOptions)));
builder.Services.Configure<TelegramSettings>(builder.Configuration.GetSection(nameof(TelegramSettings)));

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



app.UseSwagger();
app.UseSwaggerUI();
app.Urls.Add("http://0.0.0.0:80");

// app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

//seed - created user admin
//Background Task - Hangfire
using (var scope = app.Services.CreateScope())
{
    //seed
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    var serviceProvider = scope.ServiceProvider;
    await context.Database.MigrateAsync();
    await SeedAdmin.Initialize(serviceProvider);
    
    //Background Task
    var recurringJobManager = serviceProvider.GetRequiredService<IRecurringJobManager>();

    recurringJobManager.AddOrUpdate<LowStockTelegramService>(
        "send-message-telegram",
        service => service.SendMessage(),
        "0 0 * * *");
    
    recurringJobManager.AddOrUpdate<RemoveCategories>(
        "delete-categories",
        service => service.RemoveRange(),
        "20 0 * * *");
    
    recurringJobManager.AddOrUpdate<SendFinanceService>(
        "send-finance",
        service => service.SendMail(),
        "0 0 1 * *");
}
// open file from chrome
app.UseHangfireDashboard();
app.UseStaticFiles();
app.MapControllers();
app.Run();