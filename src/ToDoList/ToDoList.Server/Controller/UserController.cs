//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using System.Security.Claims;
//using ToDoList.Bll.DTOs;
//using ToDoList.Bll.Services;

//namespace ToDoList.Server.Controller;

//[Authorize]
//[Route("api/user")]
//[ApiController]
//public class UserController : ControllerBase
//{
//    private readonly IUserService UserService;
//    public UserController(IUserService userService)
//    {
//        UserService = userService;
//    }

//    [Authorize(Roles = "Admin,SuperAdmin")]
//    [HttpDelete("delete")]
//    public async Task DeleteUserById(long userId)
//    {
//        var role = User.FindFirst(ClaimTypes.Role)?.Value;
//        await UserService.DeleteUserByIdAsync(userId, role);
//    }

//    [Authorize(Roles = "SuperAdmin")]
//    [HttpPatch("updateRole")]
//    public async Task UpdateRole(long userId, UserRoleDto userRoleDto)
//    {
//        await UserService.UpdateUserRoleAsync(userId, userRoleDto);
//    }
//}
