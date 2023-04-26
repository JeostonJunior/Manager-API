using AutoMapper;
using Manager.API.Utilities;
using Manager.API.ViewModels;
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

var jwtSecretKey = builder.Configuration.GetSection("Jwt").GetValue<string>("Key");

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
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSecretKey)),
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

builder.Services.AddSingleton(autoMapperConfig.CreateMapper());

#endregion

#region Swagger

builder.Services.AddSwaggerGen(config =>
{
    config.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Manager API",
        Version = "v1",
        Description = "Database user management API using Automapper, DTO Model, Repository Pattern and SQLServer",
        Contact = new OpenApiContact
        {
            Name = "Jeoston Araujo",
            Email = "jeostonjunior@gmail.com",
            Url = new Uri("https://www.linkedin.com/in/jeoston-araujo/")
        }
    });

    config.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please use Bearer <TOKEN>",
        Name = "Authorization",
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
                            Id = "Bearer"
                        }
            },
            Array.Empty<string>()
        }
    });
});

#endregion

#region DataBase

var connectionString = builder.Configuration.GetConnectionString("ManagerBD");

builder.Services.AddDbContext<ManagerContext>(options => options.UseSqlServer(connectionString).EnableSensitiveDataLogging()
                .UseLoggerFactory(LoggerFactory.Create(bld => bld.AddConsole())), ServiceLifetime.Transient);

#endregion

#region Dependency Injection

builder.Services.AddScoped<IUserService, UserService>()
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
