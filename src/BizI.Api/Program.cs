using System.Text;
using BizI.Api.Endpoints;
using BizI.Api.Middleware;
using BizI.Application;
using BizI.Infrastructure.Data.Migrations;
using BizI.Infrastructure.Data.Seeding;
using BizI.Infrastructure.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Scalar.AspNetCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// ── Logging ──────────────────────────────────────────────────────────────────
builder.Host.UseSerilog((ctx, lc) => lc.ReadFrom.Configuration(ctx.Configuration));

// ── OpenAPI / Swagger ─────────────────────────────────────────────────────────
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "BizI API", Version = "v1" });

    var bearerScheme = new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.Http,
        Name = "Authorization",
        Scheme = "bearer",
        In = ParameterLocation.Header,
        BearerFormat = "JWT",
        Description = "Enter your JWT token."
    };
    options.AddSecurityDefinition("Bearer", bearerScheme);

    var securityRequirement = new OpenApiSecurityRequirement
    {
        [new OpenApiSecurityScheme
        {
            Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
        }] = Array.Empty<string>()
    };
    options.AddSecurityRequirement(securityRequirement);
    options.OperationFilter<TenantHeaderOperationFilter>();
});

// ── Application (MediatR, FluentValidation, AutoMapper, IInventoryService) ───
// All Application-layer services registered here. Infrastructure wires DB/repos.
builder.Services.AddApplication();

// ── Infrastructure (DB, Repos, Auth, TenantProvider) ─────────────────────────
builder.Services.AddInfrastructure(builder.Configuration);

// ── Authentication ────────────────────────────────────────────────────────────
var jwtSection = builder.Configuration.GetSection("Jwt");
var signingKey = Encoding.ASCII.GetBytes(
    jwtSection["Key"] ?? "super_secret_key_12345678901234567890123456789012");

builder.Services
    .AddAuthentication(x =>
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
            IssuerSigningKey = new SymmetricSecurityKey(signingKey),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

builder.Services.AddAuthorization();

// ── Build ─────────────────────────────────────────────────────────────────────
var app = builder.Build();

// ── Migrations & Seeding (CLI flag) ──────────────────────────────────────────
if (args.Contains("--migrate"))
{
    using var scope = app.Services.CreateScope();
    var migrationRunner = scope.ServiceProvider.GetRequiredService<MigrationRunner>();
    migrationRunner.Run();

    var seeder = scope.ServiceProvider.GetRequiredService<IDataSeeder>();
    await seeder.SeedAsync();

    Console.WriteLine("Migrations and seeding complete.");
    return;
}

// ── Middleware pipeline ───────────────────────────────────────────────────────
app.UseMiddleware<CorrelationIdMiddleware>();
app.UseSerilogRequestLogging();
app.UseMiddleware<GlobalExceptionMiddleware>();
app.UseMiddleware<TenantMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger(options => options.RouteTemplate = "openapi/{documentName}.json");
    app.MapScalarApiReference();
}

app.UseAuthentication();
app.UseAuthorization();

// ── Endpoints ─────────────────────────────────────────────────────────────────
app.MapAuthEndpoints();
app.MapProductEndpoints();
app.MapOrderEndpoints();
app.MapInventoryEndpoints();
app.MapInventoryTransactionEndpoints();
app.MapCategoryEndpoints();
app.MapCustomerEndpoints();
app.MapCustomerGroupEndpoints();
app.MapSupplierEndpoints();
app.MapWarehouseEndpoints();
app.MapImportOrderEndpoints();
app.MapPaymentAndReturnEndpoints();
app.MapRoleEndpoints();
app.MapUserEndpoints();
app.MapAuditLogEndpoints();

app.Run();

// ── Swagger operation filter for tenant header ────────────────────────────────
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
