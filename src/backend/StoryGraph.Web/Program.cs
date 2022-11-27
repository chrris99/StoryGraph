using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using Microsoft.AspNetCore.Http.Connections;
using StoryGraph.Api;
using StoryGraph.Application.Hubs;
using StoryGraph.Application.Repositories;
using StoryGraph.Application.Services;
using StoryGraph.Infrastructure;
using StoryGraph.Infrastructure.Authentication;
using StoryGraph.Infrastructure.Language;
using StoryGraph.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

var policy = builder.Configuration.GetValue<string>("Cors:Policy");

builder.Services
    .AddControllers()
    .AddApplicationPart(AssemblyReference.Assembly)
    .AddControllersAsServices();

builder.Services
    .AddEndpointsApiExplorer()
    .AddSwaggerGen();

// Configure Swagger
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Story Visualizer API",
        Version = "v1"
    });
    options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
    {            
        Name = "Authorization",
        Description = "Enter a valid token",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = JwtBearerDefaults.AuthenticationScheme
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = JwtBearerDefaults.AuthenticationScheme
                }
            },
            new List<string>()
        }
    });
});

// Configure Database Connection
builder.Services
    .AddDbContext<ApplicationDbContext>(options =>
    {
        options.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
    })
    .AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

// Configure Identity Options
builder.Services.Configure<IdentityOptions>(options =>
{
    options.User.RequireUniqueEmail = true;
    options.User.AllowedUserNameCharacters =
        "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+/ ";

    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 8;
});

// Configure JWT Authentication
builder.Services
    .Configure<JwtOptions>(builder.Configuration.GetSection(JwtOptions.Key))
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        var key = Encoding.UTF8.GetBytes(builder.Configuration.GetValue<string>("Jwt:Secret"));

        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = false, // for dev
            ValidateAudience = false, // for dev
            RequireExpirationTime = false, // for dev
            ValidateLifetime = true
        };
    });

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy(policy, corsPolicyBuilder => corsPolicyBuilder
        .WithOrigins(builder.Configuration.GetValue<string>("Cors:Origin"))
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials());
});

// Configure SignalR
builder.Services.AddSignalR();

// Configure Http Clients for NLP Services
builder.Services.AddHttpClient();

// Configure Services
builder.Services
    .AddScoped<ISentenceSplitter, SentenceSplitter>()
    .AddScoped<ITokenEnricher, TokenEnricher>()
    .AddScoped<INamedEntityRecognizer, NamedEntityRecognizer>()
    .AddScoped<IVisualizationService, GraphVisualizationService>()
    .AddScoped<ITokenProvider, JwtProvider>()
    .AddScoped<IUserRepository, UserRepository>()
    .AddScoped<IStoryRepository, StoryRepository>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseCors(policy);

app.MapControllers();

app.MapHub<StoryHub>("/hub", options =>
{
    options.Transports = HttpTransportType.WebSockets;
});

app.Run();