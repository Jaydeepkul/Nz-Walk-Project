using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using NZWalks.Data;
using NZWalks.Mappings;
using NZWalks.Repositories;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.OpenApi.Models;
using System.Security.Cryptography.Xml;
using System.Data;
using Microsoft.Extensions.FileProviders;
using Serilog;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("Logs/NZWalks_Logs.txt",rollingInterval: RollingInterval.Day)
    .MinimumLevel.Information()
    .MinimumLevel.Warning()
    .CreateLogger();

builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);

builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "NZ Walks API",
        Version = "v1"
    });

    options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
    {
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = JwtBearerDefaults.AuthenticationScheme
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id = JwtBearerDefaults.AuthenticationScheme
                },
                Scheme = "Oauth2",
                Name= JwtBearerDefaults.AuthenticationScheme,
                In = ParameterLocation.Header
            },
            new List<string>()
        }
    });
});

builder.Services.AddDbContext<NZWalksDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("NZWalksConnectionString")));

builder.Services.AddDbContext<NZWalksAuthDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("NZWalksAuthConnectionString")));

builder.Services.AddScoped<IRegionRepository,SQLRegionRepository>();
builder.Services.AddScoped<IWalksRepository, SQLWalksRepository>();
builder.Services.AddScoped<ITokenRepository, TokenRepository>();
builder.Services.AddScoped<IImageRepository, ImageRepository>();

builder.Services.AddAutoMapper(typeof(AutoMapperProfiles));

//setup Identity
builder.Services.AddIdentityCore<IdentityUser>()
    .AddRoles<IdentityRole>()
    .AddTokenProvider<DataProtectorTokenProvider<IdentityUser>>("NZWalks")
    .AddEntityFrameworkStores<NZWalksAuthDbContext>()
    .AddDefaultTokenProviders();

//Setup Identity options
//these setting we are using in our controller for login
builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1;
});


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
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

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(),"Images")),
    RequestPath = "/Images"
});

app.MapControllers();

app.Run();
