﻿using Azure.Core;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using ToDoList.Bll.DTOs;
using ToDoList.Bll.Services.Helpers;
using ToDoList.Bll.Services.Helpers.Security;
using ToDoList.Core.Errors;
using ToDoList.Dal.Entity;
using ToDoList.Errors;
using ToDoList.Repository.ToDoItemRepository;
using System.Security.Claims;

namespace ToDoList.Bll.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository UserRepository;
    private readonly IRefreshTokenRepository RefreshTokenRepository;
    private readonly ITokenService TokenService;



    public AuthService(ITokenService tokenService, IUserRepository userRepository, IRefreshTokenRepository refreshTokenRepository)
    {
        TokenService = tokenService;
        UserRepository = userRepository;
        RefreshTokenRepository = refreshTokenRepository;
    }

    public async Task<LoginResponseDto> LoginUserAsync(UserLoginDto userLoginDto)
    {
        var user = await UserRepository.SelectUserByUserNameAsync(userLoginDto.UserName);

        var checkUserPassword = PasswordHasher.Verify(userLoginDto.Password, user.Password, user.Salt);

        if (checkUserPassword == false)
        {
            throw new UnauthorizedException("UserName or password incorrect");
        }

        var userGetDto = new UserGetDto()
        {
            UserId = user.UserId,
            UserName = user.UserName,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber,
            Role = (UserRoleDto)user.Role,
        };

        var token = TokenService.GenerateToken(userGetDto);
        var refreshToken = TokenService.GenerateRefreshToken();

        var refreshTokenToDB = new RefreshToken()
        {
            Token = refreshToken,
            Expires = DateTime.UtcNow.AddDays(21),
            IsRevoked = false,
            UserId = user.UserId
        };

        await RefreshTokenRepository.AddRefreshTokenAsync(refreshTokenToDB);

        var loginResponseDto = new LoginResponseDto()
        {
            AccessToken = token,
            RefreshToken = refreshToken,
            TokenType = "Bearer",
            Expires = 24
        };
       

        return loginResponseDto;
    }

    public async Task LogOutAsync(string token)
    {
        await RefreshTokenRepository.RemoveRefreshTokenAsync(token);
    }

    public async Task<LoginResponseDto> RefreshTokenAsync(RefreshRequestDto request)
    {
        ClaimsPrincipal? principal = TokenService.GetPrincipalFromExpiredToken(request.AccessToken);
        if (principal == null) throw new ForbiddenException("Invalid access token.");

        
        var userClaim = principal.FindFirst(c => c.Type == "UserId");
        var userId = long.Parse(userClaim.Value);


        var refreshToken = await RefreshTokenRepository.SelectRefreshTokenAsync(request.RefreshToken, userId);
        if (refreshToken == null || refreshToken.Expires < DateTime.UtcNow || refreshToken.IsRevoked)
            throw new UnauthorizedException("Invalid or expired refresh token.");

        refreshToken.IsRevoked = true;

        var user = await UserRepository.SelectUserByIdAsync(userId);

        var userGetDto = new UserGetDto()
        {
            UserId = user.UserId,
            UserName = user.UserName,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber,
            Role = (UserRoleDto)user.Role,
        };

        var newAccessToken = TokenService.GenerateToken(userGetDto);
        var newRefreshToken = TokenService.GenerateRefreshToken();

        var refreshTokenToDB = new RefreshToken()
        {
            Token = newRefreshToken,
            Expires = DateTime.UtcNow.AddDays(21),
            IsRevoked = false,
            UserId = user.UserId
        };

        await RefreshTokenRepository.AddRefreshTokenAsync(refreshTokenToDB);

        return new LoginResponseDto
        {
            AccessToken = newAccessToken,
            RefreshToken = newRefreshToken,
            TokenType = "Bearer",
            Expires = 900
        };
    }

    public async Task<long> SignUpUserAsync(UserCreateDto userCreateDto)
    {
        var tupleFromHasher = PasswordHasher.Hasher(userCreateDto.Password);
        var user = new User()
        {
            FirstName = userCreateDto.FirstName,
            LastName = userCreateDto.LastName,
            UserName = userCreateDto.UserName,
            Email = userCreateDto.Email,
            PhoneNumber = userCreateDto.PhoneNumber,
            Password = tupleFromHasher.Hash,
            Salt = tupleFromHasher.Salt,
            Role = UserRole.User,
        };

        return await UserRepository.InsertUserAsync(user);
    }
}
