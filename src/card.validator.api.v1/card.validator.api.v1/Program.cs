// -------------------------------------------------------------------------------------------------
//
// Program.cs -- The Program.cs class.
//
// Copyright (c) 2025 Krishneel Kumar. All rights reserved.
//
// -------------------------------------------------------------------------------------------------

using System.Reflection;
using card.validator.api.v1.Endpoints;
using card.validator.api.v1.Interfaces;
using card.validator.api.v1.Services;

var builder = WebApplication.CreateBuilder(args);

// MediatR
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

// Read CORS origins directly from config
var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>();

// AutoMapper
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());
// Add services to the container.
builder.Services.AddScoped<ICardValidationService, CardValidationService>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowConfiguredOrigins", policy =>
    {
        policy.WithOrigins(allowedOrigins!)
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});


var app = builder.Build();

// Apply CORS
app.UseCors("AllowConfiguredOrigins");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

else
{
    app.UseHttpsRedirection();
    // TODO: Add OWASP recommended headers for production hardening
    // Use `OwaspHeaders.Core` NuGet package:
    // https://www.nuget.org/packages/OwaspHeaders.Core/
    // app.UseOwaspHeaders();  // Enable after installing the package
}

app.MapValidateCardEndpoint();


app.Run();
