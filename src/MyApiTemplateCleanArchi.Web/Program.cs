using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using MyApiTemplateCleanArchi.Application.Services.Interfaces;
using MyApiTemplateCleanArchi.Application.Services;
using MyApiTemplateCleanArchi.Domain.Interfaces;
using MyApiTemplateCleanArchi.Infrastructure.Persistence.Repositories;
using MyApiTemplateCleanArchi.Infrastructure.Persistence;
using Microsoft.OpenApi.Models;
using MyApiTemplateCleanArchi.Web.Middlewares;
using Serilog.Events;
using Serilog;
using MyApiTemplateCleanArchi.Application.Modules.Interfaces;
using MyApiTemplateCleanArchi.Application.Modules.Mediator;
using MyApiTemplateCleanArchi.Application.DTOs;
using MyApiTemplateCleanArchi.Application.Queries.GetUser;
using MyApiTemplateCleanArchi.Application.Queries.GetAllUsers;
using MyApiTemplateCleanArchi.Shared.Commons.Pagination;
using MyApiTemplateCleanArchi.Application.Commands.Auth;
using MyApiTemplateCleanArchi.Application.DTOs.Auth;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

var configuration = builder.Configuration;

var connectionString = configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ITokenService, TokenService>();

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

// Register the command and query handlers
builder.Services.AddTransient<IRequestHandler<GetUserQuery, UserDto>, GetUserQueryHandler>();
builder.Services.AddTransient<IRequestHandler<GetAllUsersQuery, PagedList<UserDto>>, GetAllUsersQueryHandler>();

// Register the authentication commands
builder.Services.AddTransient<IRequestHandler<LoginCommand, AuthResponseDto>, LoginCommandHandler>();
builder.Services.AddTransient<IRequestHandler<RegisterCommand, string>, RegisterCommandHandler>();

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
