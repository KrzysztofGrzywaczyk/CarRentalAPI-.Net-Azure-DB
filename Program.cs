using CarRentalAPI;
using CarRentalAPI.Configuration;
using CarRentalAPI.Entities;
using CarRentalAPI.Handlers;
using CarRentalAPI.Middlewares;
using CarRentalAPI.Models;
using CarRentalAPI.Models.Validators;
using CarRentalAPI.Services;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Identity;
using NLog;
using NLog.Config;
using System.Text;
using Microsoft.IdentityModel.Tokens;

LogManager.Configuration = new XmlLoggingConfiguration("nlog.config");


var builder = WebApplication.CreateBuilder(args);
var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .AddUserSecrets<Program>()
            .Build();
var logger = LogManager.GetCurrentClassLogger();
var authenticationSettings = new AuthenticationSettings();
config.GetSection("Authentication").Bind(authenticationSettings);
builder.Services.AddSingleton(authenticationSettings);

builder.Services.AddAuthentication(option =>
{
    option.DefaultAuthenticateScheme = "Bearer";
    option.DefaultScheme = "Bearer";
    option.DefaultChallengeScheme = "Bearer";
}).AddJwtBearer(cfg =>
{
    cfg.RequireHttpsMetadata = false;
    cfg.SaveToken = true;
    cfg.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidIssuer = authenticationSettings.JwtIssuer,
        ValidAudience = authenticationSettings.JwtIssuer,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authenticationSettings.JwtKey))
    };
});

builder.Services.AddControllers(); 
builder.Services.AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<ICarService, CarService>();
builder.Services.AddTransient<ILogHandler, LogHandler>();
builder.Services.AddTransient<IRentalService, RentalService>();
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddSingleton(config.Get<RentalDbContextConfiguration>() ?? throw new ArgumentNullException(nameof(config), "Configuration is required to retrieve RentalDbContextConfig"));
builder.Services.AddDbContext<RentalDbContext>();
builder.Services.AddScoped<RentalSeeder>();
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
builder.Services.AddScoped<IValidator<AddUserDto>, AddUserValidator>();

builder.Services.AddScoped<ErrorHandlingMiddleware>();
builder.Services.AddScoped<RequestTimeMiddleware>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

/*using (var scope = app.Services.CreateScope())
{
    var seeder = scope.ServiceProvider.GetRequiredService<RentalSeeder>();
    seeder.SeedBasicRoles();
}*/


app.UseMiddleware<ErrorHandlingMiddleware>();
app.UseMiddleware<RequestTimeMiddleware>();

app.UseAuthentication();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
