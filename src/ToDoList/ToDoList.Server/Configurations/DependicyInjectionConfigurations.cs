using FluentValidation;
using ToDoList.Bll.DTOs;
using ToDoList.Bll.Services;
using ToDoList.Bll.Services.Helpers;
using ToDoList.Bll.Validators;
using ToDoList.Repository.ToDoItemRepository;

namespace ToDoList.Server.Configurations;

public static class DependicyInjectionConfigurations
{
    public static void Configure(this WebApplicationBuilder builder)
    {
        //builder.Services.AddScoped<IToDoItemRepository, AdoNetWithSpAndFn>();
        builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();

        builder.Services.AddScoped<IToDoItemRepository, ToDoItemRepository>();
        builder.Services.AddScoped<IToDoItemService, ToDoItemService>();

        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddScoped<IAuthService, AuthService>();
        builder.Services.AddScoped<IUserService, UserService>();

        builder.Services.AddScoped<ITokenService, TokenService>();

        builder.Services.AddScoped<ToDoItemUpdateDtoValidator, ToDoItemUpdateDtoValidator>();
        builder.Services.AddScoped<ToDoItemCreateDtoValidator, ToDoItemCreateDtoValidator>();

        builder.Services.AddScoped<IValidator<ToDoItemCreateDto>, ToDoItemCreateDtoValidator>();
        builder.Services.AddScoped<IValidator<ToDoItemUpdateDto>, ToDoItemUpdateDtoValidator>();




    }
}
