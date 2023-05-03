using AutoMapper;
using Manager.API.Utilities;
using Manager.API.ViewModels;
using Manager.Domain.Constants;
using Manager.Domain.Entities;
using Manager.Infra.Context;
using Manager.Infra.Interfaces;
using Manager.Infra.Repositories;
using Manager.Services.DTOS;
using Manager.Services.Interfaces;
using Manager.Services.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

#region JWT

var settings = builder.Configuration.GetSection(Constants.SETTINGS_SECTION).Get<ApiSettings>();

builder.Services.AddAuthentication(auth =>
{
    auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(auth =>
{
    auth.RequireHttpsMetadata = false;
    auth.SaveToken = true;
    auth.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        ValidateIssuer = false,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(settings.JwtSettings.Key)),
        ValidateAudience = false
    };
});

#endregion

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

#region Mapper

var autoMapperConfig = new MapperConfiguration(config =>
{
    config.CreateMap<User, UserDTO>().ReverseMap();
    config.CreateMap<CreateUserViewModel, UserDTO>().ReverseMap();
    config.CreateMap<UpdateUserViewModel, UserDTO>().ReverseMap();
});

#endregion

#region Swagger

builder.Services.AddSwaggerGen(config =>
{
    config.SwaggerDoc(Constants.SWAGGER_VERSION, new OpenApiInfo
    {
        Title = Constants.SWAGGER_TITLE,
        Version = Constants.SWAGGER_VERSION,
        Description = Constants.SWAGGER_DESCRIPTION,
        Contact = new OpenApiContact
        {
            Name = Constants.SWAGGER_CONTACT_NAME,
            Email = Constants.SWAGGER_CONTACT_EMAIL,
            Url = new Uri(Constants.SWAGGER_CONTACT_URL)
        }
    });

    config.AddSecurityDefinition(Constants.SWAGGER_BEARER, new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = Constants.SWAGGER_SECURITY_DESCRIPTION,
        Name = Constants.SWAGGER_SECURITY_NAME,
        Type = SecuritySchemeType.ApiKey
    });

    config.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = Constants.SWAGGER_BEARER
                        }
            },
            Array.Empty<string>()
        }
    });
});

#endregion

#region DataBase

builder.Services.AddDbContext<ManagerContext>(options => options.UseSqlServer(settings.ConnectionString.ManagerBD).EnableSensitiveDataLogging()
                .UseLoggerFactory(LoggerFactory.Create(bld => bld.AddConsole())), ServiceLifetime.Transient);

#endregion

#region Dependency Injection

builder.Services.AddSingleton(settings)
                .AddSingleton(autoMapperConfig.CreateMapper())
                .AddScoped<IUserService, UserService>()
                .AddScoped<IUserRepository, UserRepository>()
                .AddSingleton<ITokenGenerator, TokenGenerator>();

#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
