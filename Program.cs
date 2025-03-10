using DotNetEnv;
using compulsoryRest.Database;
using compulsoryRest.Repositories;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using Microsoft.OpenApi.Models;


var builder = WebApplication.CreateBuilder(args);

// Load env variables
Env.Load();
var mongoDbConnectionString = Environment.GetEnvironmentVariable("MONGO_CONNECTION_STRING")!;

// Add environment variables to the configuration
builder.Configuration.AddEnvironmentVariables();

// Add services to the container
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
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Compulsory REST API",
        Version = "v1",
        Description = "A simple API to manage movies and users"
    });

    // Enable XML comments
    var xmlFile = Path.Combine(AppContext.BaseDirectory, "compulsory.xml");
    options.IncludeXmlComments(xmlFile);
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
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

builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please insert JWT with Bearer into field",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

builder.Services.AddAuthorization();  // To use authorization


var app = builder.Build();

// test data seeder
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<MongoDbContext>();
    var seeder = new DatabaseSeeder(dbContext);
    seeder.SeedMovies(20); // Generate 20 test movies
}

// Enable CORS
app.UseCors("AllowAll");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "Compulsory REST API v1");
            options.RoutePrefix = string.Empty;
        });
}

app.UseAuthentication(); // Add authentication middleware
app.UseAuthorization();  // Add authorization middleware

// Map controllers
app.MapControllers();

app.Run();