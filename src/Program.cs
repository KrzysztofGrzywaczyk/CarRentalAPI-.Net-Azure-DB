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
using Microsoft.AspNetCore.Authorization;
using System.Reflection;
using CarRentalAPI.Authorization;
using CarRentalAPI.Authorizationl;
using CarRentalAPI.Models.Queries;
using CarRentalAPI.Models.Pagination;

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
builder.Services.AddScoped<IAuthorizationHandler, RentalResourceOperationRequirementHandler>();
builder.Services.AddScoped<IAuthorizationHandler, CarResourceOperationRequirementHandler>();
builder.Services.AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "CarRentalAPI", Version = "v1" });
    var securityScheme = new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = "JWT Authorization header using the Bearer scheme",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        Reference = new Microsoft.OpenApi.Models.OpenApiReference
        {
            Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
            Id = "Bearer"
        }
    };
    c.AddSecurityDefinition("Bearer", securityScheme);
    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        { securityScheme, new List<string>() }
    });
});
builder.Services.AddTransient<ICarService, CarService>();
builder.Services.AddTransient<ILogHandler, LogHandler>();
builder.Services.AddTransient<IRentalService, RentalService>();
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddSingleton(config.Get<RentalDbContextConfiguration>() ?? throw new ArgumentNullException(nameof(config), "Configuration is required to retrieve RentalDbContextConfig"));
builder.Services.AddDbContext<RentalDbContext>();
builder.Services.AddScoped<RentalSeeder>();
builder.Services.AddScoped<IUserContextService, UserContextService>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
builder.Services.AddScoped<IValidator<CreateUserDto>, CreateUserValidator>();
builder.Services.AddScoped<IValidator<UpdateUserDto>, UpdateUserValidator>();
builder.Services.AddScoped<IValidator<CarQuery>,  CarQueryValidator>();
builder.Services.AddScoped<IValidator<RentalQuery>, RentalQueryValidator>();

builder.Services.AddScoped<ErrorHandlingMiddleware>();
builder.Services.AddScoped<RequestTimeMiddleware>();

builder.Services.AddCors(options => 
{
    options.AddPolicy("FrontendClient", builder =>
    {
    builder.AllowAnyMethod()
    .AllowAnyHeader()
    .WithOrigins(config["AllowedOrigins"] ?? throw new ArgumentNullException(nameof(config), "Configuration is required to retrieve RentalDbContextConfig"));
});
});

var app = builder.Build();

app.UseCors("FrontendClient");
using (var scope = app.Services.CreateScope())
{
    var seeder = scope.ServiceProvider.GetRequiredService<RentalSeeder>();
    seeder.SeedBasicRoles();
}

app.UseMiddleware<ErrorHandlingMiddleware>();
app.UseMiddleware<RequestTimeMiddleware>();

app.UseAuthentication();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.UseAuthorization();

app.Run();
