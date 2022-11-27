using FluentValidation;
using StoryGraph.Validation.Domain;
using StoryGraph.Validation.Web.Validators;

const string clientCorsPolicy = "ClientCorsPolicy";

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    var origin = builder.Configuration.GetValue<string>("Cors:Origin");
    
    options.AddPolicy(clientCorsPolicy, policyBuilder =>
    {
        policyBuilder
            .WithOrigins(origin)
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
    });
});

builder.Services.AddScoped<IValidator<Story>, StoryValidator>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(clientCorsPolicy);

app.MapControllers();

app.Run();