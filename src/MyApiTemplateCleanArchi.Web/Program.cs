using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using MyApiTemplateCleanArchi.Application.Services.Interfaces;
using MyApiTemplateCleanArchi.Application.Services;
using MyApiTemplateCleanArchi.Infrastructure.Persistence;
using Microsoft.OpenApi.Models;
using MyApiTemplateCleanArchi.Web.Middlewares;
using Serilog.Events;
using Serilog;
using MyApiTemplateCleanArchi.Application.Modules.Interfaces;
using MyApiTemplateCleanArchi.Application.Modules.Mediator;
using MyApiTemplateCleanArchi.Application.Queries.GetUser;

using MyApiTemplateCleanArchi.Infrastructure.Services;
using MyApiTemplateCleanArchi.Application;
using MyApiTemplateCleanArchi.Domain.Interfaces;
using MyApiTemplateCleanArchi.Infrastructure.Persistence.Repositories;
using MyApiTemplateCleanArchi.Infrastructure.Persistence.DbContexts;


var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

var configuration = builder.Configuration;
// SQL Server
var sqlConnStr = configuration.GetConnectionString("ApplicationDbContext");
builder.Services.AddDbContext<ApplicationDbContext>(opts =>
    opts.UseSqlServer(sqlConnStr));

//   � PostgreSQL
var pgConnStr = configuration.GetConnectionString("TodoPostgreDbContext");
builder.Services.AddDbContext<TodoPostgreDbContext>(opts =>
    opts.UseNpgsql(pgConnStr));

// Add DbContext and repositories
builder.Services.AddPersistenceInfrastructure(builder.Configuration);

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ITokenService, TokenService>();

// Add interfaces and implementations
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ITodoRepository, TodoRepository>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = configuration["Jwt:Issuer"],
            ValidAudience = configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]))
        };
    });
builder.Services.AddAuthorization();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddScoped<IMediator, SimpleMediator>();

// Add auto-implementation of handlers
builder.Services.AddHandlersFromAssembly(typeof(GetUserQueryHandler).Assembly);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "MyApiTemplate", Version = "v1" });
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Entrez 'Bearer' suivi d'un espace et de votre token",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference{ Type = ReferenceType.SecurityScheme, Id = "Bearer" }
            },
            Array.Empty<string>()
        }
    });
});

builder.Host.UseSerilog();

var app = builder.Build();

// Automatically apply migrations on startup, and databases
using (var scope = app.Services.CreateScope())
{
    var allContexts = scope.ServiceProvider.GetServices<DbContext>();
    foreach (var db in allContexts)
    {
        db.Database.Migrate();
    }
}

app.UseMiddleware<ExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
