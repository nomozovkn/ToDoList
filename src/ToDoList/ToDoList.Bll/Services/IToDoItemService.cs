using ToDoList.Bll.DTOs;

namespace ToDoList.Bll.Services
{
    public interface IToDoItemService
    {
        Task<List<ToDoItemGetDto>> GetByDueDateAsync(DateTime dueDate);
        Task<ToDoItemGetDto> GetToDoItemByIdAsync(long id);
        Task<GetAllResponseModel> GetAllToDoItemsAsync(int skip, int take, long userId);
        Task<List<ToDoItemGetDto>> GetAllToDoItemsAsync(long userId);
        Task<long> AddToDoItemAsync(ToDoItemCreateDto toDoItem, long userId);
        Task DeleteToDoItemByIdAsync(long id);
        Task UpdateToDoItemAsync(ToDoItemUpdateDto newItem, long userId);
        Task<GetAllResponseModel> GetCompletedAsync(int skip, int take);
        Task<GetAllResponseModel> GetIncompleteAsync(int skip, int take);
        Task<int> GetTotalCountAsync();
        Task<GetAllResponseModel> GetPagedFilteredToDoItemsAsync(ToDoFilterParams toDoFilterParams, long userId);

    }
}
