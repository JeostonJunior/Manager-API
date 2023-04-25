using AutoMapper;
using Manager.API.ViewModels;
using Manager.Domain.Entities;
using Manager.Infra.Context;
using Manager.Infra.Interfaces;
using Manager.Infra.Repositories;
using Manager.Services.DTOS;
using Manager.Services.Interfaces;
using Manager.Services.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("ManagerBD");

var autoMapperConfig = new MapperConfiguration(config =>
{
    config.CreateMap<User, UserDTO>().ReverseMap();
    config.CreateMap<CreateUserViewModel, UserDTO>().ReverseMap();
});

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
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
});

builder.Services.AddSingleton(autoMapperConfig.CreateMapper());

builder.Services.AddDbContext<ManagerContext>(options => options.UseSqlServer(connectionString).EnableSensitiveDataLogging()
                .UseLoggerFactory(LoggerFactory.Create(bld => bld.AddConsole())), ServiceLifetime.Transient);

builder.Services.AddScoped<IUserService, UserService>()
                .AddScoped<IUserRepository, UserRepository>();

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

app.UseAuthorization();

app.MapControllers();

app.Run();
