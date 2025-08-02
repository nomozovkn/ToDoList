using ToDoList.Dal.Entity;

namespace ToDoList.Repository.ToDoItemRepository;

public interface IRefreshTokenRepository
{
    Task AddRefreshTokenAsync(RefreshToken refreshToken);
    Task<RefreshToken> SelectRefreshTokenAsync(string refreshToken, long userId);
    Task RemoveRefreshTokenAsync(string token);
}