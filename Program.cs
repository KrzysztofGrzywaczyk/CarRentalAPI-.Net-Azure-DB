using CarRentalAPI;
using CarRentalAPI.Configuration;
using CarRentalAPI.Entities;
using CarRentalAPI.Handlers;

var builder = WebApplication.CreateBuilder(args);
var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .AddUserSecrets<Program>()
            .Build(); 

builder.Services.AddControllers();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<IDeleteRentalHandler, DeleteRentalHandler>();
builder.Services.AddTransient<IGetRentalHandler, GetRentalHandler>();
builder.Services.AddTransient<IPostRentalHandler, PostRentalHandler>();
builder.Services.AddTransient<IPutRentalHandler, PutRentalHandler>();
builder.Services.AddSingleton(config.Get<RentalDbContextConfiguration>() ?? throw new ArgumentNullException(nameof(config), "Configuration is required to retrieve RentalDbContextConfig"));
builder.Services.AddDbContext<RentalDbContext>();
builder.Services.AddScoped<RentalSeeder>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// RentalSeeder.Seed();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
