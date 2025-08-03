using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using OnlineEdu.Data.Abstract;
using OnlineEdu.Data.Concrete;
using OnlineEdu.Data.EduContext;
using OnlineEdu.Entity.Models;
using OnlineEdu.Map;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configuration Models
builder.Services.Configure<JwtSettings>(
    builder.Configuration.GetSection("JwtSettings"));

builder.Services.Configure<AdminCredentials>(
    builder.Configuration.GetSection("AdminCredentials"));

// Database
builder.Services.AddDbContext<EduDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlConnection")));

// Identity
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequiredLength = 8;
})
.AddEntityFrameworkStores<EduDbContext>()
.AddDefaultTokenProviders();

// JWT Authentication
var jwtSection = builder.Configuration.GetSection("JwtSettings");

// Debug for checking if the section exists
Console.WriteLine($"JwtSettings section exists: {jwtSection.Exists()}");
Console.WriteLine($"SecretKey: {jwtSection["SecretKey"]}");
Console.WriteLine($"Issuer: {jwtSection["Issuer"]}");
Console.WriteLine($"Audience: {jwtSection["Audience"]}");

var secretKey = jwtSection["SecretKey"];
var issuer = jwtSection["Issuer"];
var audience = jwtSection["Audience"];

// Control null
if (string.IsNullOrEmpty(secretKey) || string.IsNullOrEmpty(issuer) || string.IsNullOrEmpty(audience))
{
    throw new InvalidOperationException("JWT settings are missing. Check the JWT Settings section in appsettings.json.");
}

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
        ValidateIssuer = true,
        ValidIssuer = issuer,
        ValidateAudience = true,
        ValidAudience = audience,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
});

// Authorization Policies
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("SuperAdminOnly", policy => policy.RequireRole("SuperAdmin"));
    options.AddPolicy("WriterOrAbove", policy => policy.RequireRole("SuperAdmin", "Writer"));
    options.AddPolicy("AllAPIUsers", policy => policy.RequireRole("SuperAdmin", "Writer", "User"));
});

// Repository Pattern
builder.Services.AddScoped(typeof(IGeneric<>), typeof(GenericManager<>));

// Auth Services
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IJwtService, JwtService>();

// AutoMapper
builder.Services.AddAutoMapper(configmap =>
{
    configmap.AddProfile<UniversalMap>();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication(); 
app.UseAuthorization();  

app.MapControllers();

app.Run();