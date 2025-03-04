using DotNetEnv;
using compulsoryRest.Database;
using compulsoryRest.Repositories;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using Microsoft.OpenApi.Models;


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

// Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder => 
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader());
});

builder.Services.AddScoped<MovieRepository>();
builder.Services.AddScoped<UserRepository>();

// Add MongoDB connection service (singleton)
builder.Services.AddSingleton<MongoDbContext>(sp =>
    new MongoDbContext(mongoDbConnectionString)
);

// Add Swagger services
builder.Services.AddSwaggerGen(options =>
{
    // Optional: You can set up descriptions for endpoints and models
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Compulsory REST API",
        Version = "v1",
        Description = "A simple API to manage movies and users"
    });
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme) // This line requires JwtBearerDefaults - weird with .net 8
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidIssuer = "localhost",
            ValidAudience = "localhost",
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT_SECRET"]!))
        };
    });

builder.Services.AddAuthorization();  // To use authorization


var app = builder.Build();

// Enable CORS
app.UseCors("AllowAll");

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
