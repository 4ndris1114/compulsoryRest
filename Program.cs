using DotNetEnv;
using compulsoryRest.Database;
using compulsoryRest.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Load environment variables
Env.Load();

// Access the MongoDB connection string
var mongoDbConnectionString = Environment.GetEnvironmentVariable("MONGO_CONNECTION_STRING");

// If the environment variable is not set, use a default connection string (for local development)
if (string.IsNullOrEmpty(mongoDbConnectionString)) {
    mongoDbConnectionString = "mongodb://localhost:27017"; // Fallback for local development
}

// Add environment variables to the configuration
builder.Configuration.AddEnvironmentVariables();

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddScoped<MovieRepository>();
builder.Services.AddScoped<UserRepository>();

// Add MongoDB connection service (singleton)
builder.Services.AddSingleton<MongoDbContext>(sp =>
    new MongoDbContext(mongoDbConnectionString)
);

// Add Swagger services
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication(); // Add authentication middleware
app.UseAuthorization();  // Add authorization middleware

// Map controllers
app.MapControllers();

app.Run();
