using System.Text;
using BizI.Api.Endpoints;
using BizI.Api.Middleware;
using BizI.Application.Interfaces;
using BizI.Application.Services;
using BizI.Application.Common;
using BizI.Domain.Entities;
using BizI.Domain.Enums;
using BizI.Domain.Interfaces;
using BizI.Infrastructure.Auth;
using BizI.Infrastructure.Data;
using BizI.Infrastructure.Data.Migrations;
using BizI.Infrastructure.Data.Seeding;
using BizI.Infrastructure.Services;
using LiteDB;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Scalar.AspNetCore;
using Serilog;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.OpenApi;

var builder = WebApplication.CreateBuilder(args);

// Serilog
builder.Host.UseSerilog((ctx, lc) => lc
    .ReadFrom.Configuration(ctx.Configuration));

builder.Services.AddEndpointsApiExplorer();

// OpenAPI
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "BizI API",
        Version = "v1"
    });

    var scheme = new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.Http,
        Name = "Authorization",
        Scheme = "bearer",
        In = ParameterLocation.Header,
        BearerFormat = "JWT",
        Description = "Please enter token"
    };
    options.AddSecurityDefinition("Bearer", scheme);

    var requirement = new OpenApiSecurityRequirement
    {
        [new OpenApiSecurityScheme { Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" } }] = Array.Empty<string>()
    };
    options.AddSecurityRequirement(requirement);

    options.OperationFilter<TenantHeaderOperationFilter>();
});

// LiteDB
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? "biz_i.db";
var appDbContext = new AppDbContext(connectionString);
builder.Services.AddSingleton(appDbContext);
builder.Services.AddSingleton<ILiteDatabase>(appDbContext.Database);

// DI
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ITenantProvider, TenantProvider>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(LiteDbRepository<>));
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IInventoryService, InventoryService>();

// Migrations and Seeding
builder.Services.AddTransient<IMigration, InitialMigration>();
builder.Services.AddScoped<MigrationRunner>();
builder.Services.AddScoped<IDataSeeder, DataSeeder>();

// Validators
builder.Services.AddValidatorsFromAssembly(typeof(BizI.Application.Common.CommandResult).Assembly);

// MediatR
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(BizI.Application.Common.CommandResult).Assembly);
    cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
});

// Auth
var jwtSettings = builder.Configuration.GetSection("Jwt");
var key = Encoding.ASCII.GetBytes(jwtSettings["Key"] ?? "super_secret_key_12345678901234567890123456789012");
builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});
builder.Services.AddAuthorization();

var app = builder.Build();

// Run Migrations and Seeding from CLI if requested
if (args.Contains("--migrate"))
{
    using (var scope = app.Services.CreateScope())
    {
        var migrationRunner = scope.ServiceProvider.GetRequiredService<MigrationRunner>();
        migrationRunner.Run();

        var seeder = scope.ServiceProvider.GetRequiredService<IDataSeeder>();
        await seeder.SeedAsync();
    }
    Console.WriteLine("Migrations and seeding complete.");
    return;
}

app.UseMiddleware<CorrelationIdMiddleware>();
app.UseSerilogRequestLogging();
app.UseMiddleware<GlobalExceptionMiddleware>();
app.UseMiddleware<TenantMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger(options =>
    {
        options.RouteTemplate = "openapi/{documentName}.json";
    });
    app.MapScalarApiReference();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapAuthEndpoints();
app.MapInventoryEndpoints();
app.MapOrderEndpoints();
app.MapProductEndpoints();

app.Run();

public class TenantHeaderOperationFilter : Swashbuckle.AspNetCore.SwaggerGen.IOperationFilter
{
    public void Apply(OpenApiOperation operation, Swashbuckle.AspNetCore.SwaggerGen.OperationFilterContext context)
    {
        operation.Parameters ??= new List<OpenApiParameter>();
        operation.Parameters.Add(new OpenApiParameter
        {
            Name = "X-Tenant-Id",
            In = ParameterLocation.Header,
            Required = true,
            Schema = new OpenApiSchema { Type = "string", Format = "uuid" }
        });
    }
}
