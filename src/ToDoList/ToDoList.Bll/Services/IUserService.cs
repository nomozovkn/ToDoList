using ToDoList.Bll.DTOs;

namespace ToDoList.Bll.Services;

public interface IUserService
{
    Task DeleteUserByIdAsync(long userId, string UserRole);
    Task UpdateUserRoleAsync(long userId, UserRoleDto userRoleDto);
    Task<ICollection<UserGetDto>> GetAllUsersAsync();
}