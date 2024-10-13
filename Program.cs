using Microsoft.Extensions.Options;
using MongoDB.Driver;
using StreamAPI.Service;
using brandportal_dotnet.Configuration;
using brandportal_dotnet.Data.Extensions;
using brandportal_dotnet.Data.Entities;
using brandportal_dotnet.Controllers;
using brandportal_dotnet.IService;
using brandportal_dotnet.IService.IPageBanner;
using brandportal_dotnet.Service.PageBanner;
using brandportal_dotnet.IService.IRewardProgram;
using brandportal_dotnet.Service.RewardProgram;
using Volo.Abp.BlobStoring;
using brandportal_dotnet.Contracts;
using brandportal_dotnet.IService.IFile;
using brandportal_dotnet.Service.File;

var builder = WebApplication.CreateBuilder(args);


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
builder.Services.AddScoped(typeof(IRepository<FaqGroup>), typeof(MongoRepository<FaqGroup>));
builder.Services.AddScoped(typeof(IRepository<Faq>), typeof(MongoRepository<Faq>));
builder.Services.AddScoped(typeof(IRepository<Game>), typeof(MongoRepository<Game>));
builder.Services.AddScoped(typeof(IRepository<GameCategory>), typeof(MongoRepository<GameCategory>));
builder.Services.AddScoped(typeof(IPageBannerRepository<Page>), typeof(PageBannerRepository<Page>));
builder.Services.AddScoped(typeof(IPageBannerRepository<PageBanner>), typeof(PageBannerRepository<PageBanner>));
builder.Services.AddScoped(typeof(IPageBannerRepository<PageCard>), typeof(PageBannerRepository<PageCard>));
builder.Services.AddScoped(typeof(IRewardProgramRepository<LoyRewardRedeem>),
    typeof(RewardProgramRepository<LoyRewardRedeem>));
builder.Services.AddScoped(typeof(IRewardProgramRepository<LoyRewardProgram>),
    typeof(RewardProgramRepository<LoyRewardProgram>));
builder.Services.AddScoped(typeof(IRewardProgramRepository<LoyProgram>), typeof(RewardProgramRepository<LoyProgram>));
builder.Services.AddScoped(typeof(IRewardProgramRepository<LoyRewardProduct>),
    typeof(RewardProgramRepository<LoyRewardProduct>));
builder.Services.AddScoped(typeof(IRepository<SuggestPlan>), typeof(MongoRepository<SuggestPlan>));
builder.Services.AddScoped(typeof(IRepository<Account>), typeof(MongoRepository<Account>));
builder.Services.AddScoped(typeof(IRepository<Role>), typeof(MongoRepository<Role>));
builder.Services.AddScoped(typeof(IRepository<GameRate>), typeof(MongoRepository<GameRate>));
builder.Services.AddScoped(typeof(IRepository<LoyProgram>), typeof(MongoRepository<LoyProgram>));
builder.Services.AddScoped(typeof(IRepository<LoyReward>), typeof(MongoRepository<LoyReward>));
builder.Services.AddScoped(typeof(IRepository<LoyRewardProgramGame>), typeof(MongoRepository<LoyRewardProgramGame>));
builder.Services.AddScoped(typeof(IRepository<LoyAccumulationProgram>), typeof(MongoRepository<LoyAccumulationProgram>));
builder.Services.AddScoped(typeof(IRepository<LoyRewardAccumulation>), typeof(MongoRepository<LoyRewardAccumulation>));
builder.Services.AddScoped(typeof(IRepository<LoyNotification>), typeof(MongoRepository<LoyNotification>));
builder.Services.AddScoped(typeof(IRepository<PlaceTourismCategory>), typeof(MongoRepository<PlaceTourismCategory>));
builder.Services.AddScoped(typeof(IRepository<PlaceTourismGroup>), typeof(MongoRepository<PlaceTourismGroup>));
builder.Services.AddScoped(typeof(IRepository<PlaceTourism>), typeof(MongoRepository<PlaceTourism>));


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