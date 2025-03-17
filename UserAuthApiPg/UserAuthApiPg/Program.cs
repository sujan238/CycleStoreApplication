using UserAuthApiPg.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using AutoMapper; // Ensure this is included
using UserAuthApiPg.Mappings;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers()
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore; // Handle circular references
        options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore; // Optional: Ignore null values
        options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver(); // Optional: Use camelCase
    });

builder.Services.AddDbContext<AuthDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// builder.Services.AddAutoMapper(options => options.AddProfile<CycleProfile>());

var mapperConfig = new MapperConfiguration(cfg =>
{
    cfg.AddProfile(new CycleProfile());
    cfg.AddProfile(new InventoryProfile());
});
var mapper = mapperConfig.CreateMapper();
builder.Services.AddSingleton(mapper);


// JWT Authentication (if not already present)
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new  TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });

// Authorization with Roles (if not already present)
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
    options.AddPolicy("OwnerOnly", policy => policy.RequireRole("Owner")); // New policy for owner
    options.AddPolicy("AdminOrOwner", policy => policy.RequireRole("Admin", "Owner")); // Optional: Combine roles
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();