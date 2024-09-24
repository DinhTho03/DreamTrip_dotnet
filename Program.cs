using Microsoft.Extensions.Options;
using MongoDB.Driver;
using StreamAPI.Service;
using TravelItineraryProject.Configuration;
using TravelItineraryProject.Data.Extensions;
using brandportal_dotnet.Data.Entities;
using brandportal_dotnet.Controllers;
using brandportal_dotnet.IService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IMongoDatabase>(provider =>
{
    var client = provider.GetRequiredService<IMongoClient>();
    var databaseSettings = provider.GetRequiredService<IOptions<DatabaseSettings>>().Value;
    return client.GetDatabase(databaseSettings.DatabaseName);
});


// Add services to the container.
builder.Services.Configure<DatabaseSettings>(builder.Configuration.GetSection("MongoDatabase"));
// Add MongoDB client
builder.Services.AddSingleton<IMongoClient>(provider =>
{
    var databaseSettings = provider.GetRequiredService<IOptions<DatabaseSettings>>().Value;
    return new MongoClient(databaseSettings.ConnectionString);
});

// Add MongoDB database
builder.Services.AddScoped<IMongoDatabase>(provider =>
{
    var client = provider.GetRequiredService<IMongoClient>();
    var databaseSettings = provider.GetRequiredService<IOptions<DatabaseSettings>>().Value;
    return client.GetDatabase(databaseSettings.DatabaseName);
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowOrigin",
        policy =>
        {
            policy.AllowAnyOrigin()
                                .AllowAnyHeader()
                                .AllowAnyMethod();
        });
});

builder.Services.AddScoped(typeof(IRepository<FaqGroup>), typeof(MongoRepository<FaqGroup>));
builder.Services.AddScoped<DatabaseSeeder>();
var app = builder.Build();

// Add seed
var serviceProvider = builder.Services.BuildServiceProvider();
using var scope = serviceProvider.CreateScope();
var databaseSeeder = scope.ServiceProvider.GetRequiredService<DatabaseSeeder>();
databaseSeeder.Seed();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowOrigin");
app.UseAuthorization();

app.MapControllers();

app.Run();
