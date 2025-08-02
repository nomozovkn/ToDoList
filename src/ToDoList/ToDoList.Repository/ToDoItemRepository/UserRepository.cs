using Microsoft.EntityFrameworkCore;
using ToDoList.Dal;
using ToDoList.Dal.Entity;
using ToDoList.Errors;

namespace ToDoList.Repository.ToDoItemRepository;

public class UserRepository : IUserRepository
{
    private readonly MainContext MainContext;

    public UserRepository(MainContext mainDbContext)
    {
        MainContext = mainDbContext;
    }

    public async Task<User> SelectUserByUserNameAsync(string userName)
    {
        var user = await MainContext.Users.FirstOrDefaultAsync(u => u.UserName == userName);
        if (user == null)
        {
            throw new EntityNotFoundException($"Entity with userName : {user} not found");
        }

        return user;
    }

    public async Task<long> InsertUserAsync(User user)
    {
        await MainContext.Users.AddAsync(user);
        await MainContext.SaveChangesAsync();
        return user.UserId;
    }

    public async Task<User> SelectUserByIdAsync(long id)
    {
        var user = await MainContext.Users.FirstOrDefaultAsync(u => u.UserId == id);
        if(user == null)
        {
            throw new EntityNotFoundException($"Entity with {id} not found");
        }

        return user;
    }

    public async Task UpdateUserRoleAsync(long userId, UserRole userRole)
    {
        var user = await SelectUserByIdAsync(userId);
        user.Role = userRole;
        MainContext.Users.Update(user);
        await MainContext.SaveChangesAsync();
    }

    public async Task DeleteUserByIdAsync(long userId)
    {
        var user = await SelectUserByIdAsync(userId);
        MainContext.Users.Remove(user); 
        await MainContext.SaveChangesAsync();
    }

    public IQueryable<User> SelectAllUsers()
    {
        return MainContext.Users;   
    }
}
