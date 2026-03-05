using Infrastructure.Data;
using Infrastructure.Jwt;
using Infrastructure.Seed;
using Microsoft.EntityFrameworkCore;
using WebApp.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<JwtOption>(builder.Configuration.GetSection(nameof(JwtOption)));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddRegisterSwaggerGen();

builder.Services.AddDbContext<ApplicationDbContext>(op =>
    op.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnectionString")));

builder.Services.AddRegistrationServices();

builder.Services.AddJwtRegister(builder.Configuration);
builder.Services.AddAuthorization();

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
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    await context.Database.MigrateAsync();
    await SeedAdmin.Initialize(scope.ServiceProvider);
}

app.MapControllers();
app.Run("http://localhost:5090");