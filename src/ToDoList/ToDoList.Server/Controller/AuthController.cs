using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Bll.DTOs;
using ToDoList.Bll.Services;

namespace ToDoList.Server.Controller;

[Route("api/auth")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService AuthService;
    public AuthController(IAuthService authService)
    {
        AuthService = authService;
    }

    [HttpPost("sign-up")] 
    public async Task<long> SignUp(UserCreateDto userCreateDto)
    {
        return await AuthService.SignUpUserAsync(userCreateDto);        
    }

    [HttpPost("login")]
    public async Task<LoginResponseDto> Login(UserLoginDto userLoginDto)
    {
        return await AuthService.LoginUserAsync(userLoginDto);
    }

    [HttpPost("refresh-token")]
    public async Task<LoginResponseDto> RefreshToken(RefreshRequestDto request)
    {
        return await AuthService.RefreshTokenAsync(request);
    }

    [HttpDelete("log-out")]
    public async Task LogOut(string token)
    {
        await AuthService.LogOutAsync(token);
    }
}
