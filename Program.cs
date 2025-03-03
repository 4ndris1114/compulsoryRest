using DotNetEnv;
using compulsoryRest.Repositories.database;

var builder = WebApplication.CreateBuilder(args);

// Load environment variables
Env.Load();

// Access the MongoDB connection string
var mongoDbConnectionString = Environment.GetEnvironmentVariable("MONGODB_CONNECTION_STRING");

// If the environment variable is not set, use a default connection string (for local development)
if (string.IsNullOrEmpty(mongoDbConnectionString))
{
    mongoDbConnectionString = "mongodb://localhost:27017"; // Fallback for local development
}

// Add service
builder.Services.AddControllers();

// Add MongoDB connection service (singleton)
builder.Services.AddSingleton<MongoDbContext>(sp =>
    new MongoDbContext(mongoDbConnectionString)
);

// Add the JWT Service
// builder.Services.AddSingleton<IJwtService, JwtService>();

// Add Swagger services
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
