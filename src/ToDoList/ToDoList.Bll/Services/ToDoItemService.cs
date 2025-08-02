using AutoMapper;
using FluentValidation;
using Microsoft.Extensions.Logging;
using ToDoList.Errors;
using ToDoList.Bll.DTOs;
using ToDoList.Dal.Entity;
using ToDoList.Repository.ToDoItemRepository;
using Microsoft.EntityFrameworkCore;

namespace ToDoList.Bll.Services
{
    public class ToDoItemService : IToDoItemService
    {
        private readonly IToDoItemRepository _toDoItemRepository;
        private readonly IValidator<ToDoItemCreateDto> _toDoItemCreateDtoValidator;
        private readonly IValidator<ToDoItemUpdateDto> _toDoItemUpdateDtoValidator;
        private readonly IMapper _mapper;
        private readonly ILogger<ToDoItemService> _logger;


        public ToDoItemService(IToDoItemRepository toDoItemRepository, IValidator<ToDoItemCreateDto> validator, IMapper mapper, ILogger<ToDoItemService> logger, IValidator<ToDoItemUpdateDto> toDoItemUpdateDtoValidator)
        {
            _toDoItemRepository = toDoItemRepository;
            _toDoItemCreateDtoValidator = validator;
            _mapper = mapper;
            _logger = logger;
            _toDoItemUpdateDtoValidator = toDoItemUpdateDtoValidator;
        }

        public async Task<long> AddToDoItemAsync(ToDoItemCreateDto toDoItem, long userId)
        {
            var validationResult = _toDoItemCreateDtoValidator.Validate(toDoItem);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            ArgumentNullException.ThrowIfNull(toDoItem);
            var convert = _mapper.Map<ToDoItem>(toDoItem);
            convert.UserId = userId;

            var id = await _toDoItemRepository.InsertToDoItemAsync(convert);
            return id;
        }


        public async Task<GetAllResponseModel> GetPagedFilteredToDoItemsAsync(ToDoFilterParams toDoFilterParams, long userId)
        {
            var query = _toDoItemRepository.SelectAllToDoItems();

            if(toDoFilterParams.Search != null)
            {
                query = query.Where(t => t.Title.Contains(toDoFilterParams.Search));
            }

            if(toDoFilterParams.IsCompleted.HasValue)
            {
                query = query.Where(t => t.IsCompleted == toDoFilterParams.IsCompleted);
            }

            if(toDoFilterParams.FromDueDate.HasValue)
            {
                query = query.Where(t => t.DueDate > toDoFilterParams.FromDueDate);
            }

            if(toDoFilterParams.ToDueDate.HasValue)
            {
                query = query.Where(t => t.DueDate < toDoFilterParams.ToDueDate);
            }

            if(toDoFilterParams.Skip < 0)
            {
                toDoFilterParams.Skip = 0;
            }

            if(toDoFilterParams.Take > 20 || toDoFilterParams.Take < 0)
            {
                toDoFilterParams.Take = 10;
            }

            query = query.Skip(toDoFilterParams.Skip).Take(toDoFilterParams.Take);

            var toDoItems = await query.ToListAsync();
            var totalCount = _toDoItemRepository.SelectAllToDoItems()
                .Where(x => x.UserId == userId)
                .Count();

            var toDoItemDtos = toDoItems
                .Select(item => _mapper.Map<ToDoItemGetDto>(item))
                .ToList();

            var getAllResponseModel = new GetAllResponseModel()
            {
                ToDoItemGetDtos = toDoItemDtos,
                TotalCount = totalCount,
            };

            return getAllResponseModel;
        }

        public async Task<GetAllResponseModel> GetAllToDoItemsAsync(int skip, int take, long userId)
        {
            var query = _toDoItemRepository.SelectAllToDoItems();
            query = query.Where(x => x.UserId == userId);
            query = query.Skip(skip).Take(take);

            var toDoItems = await query.ToListAsync();
            var totalCount = _toDoItemRepository.SelectAllToDoItems()
                .Where(x => x.UserId == userId)
                .Count();

            var toDoItemDtos = toDoItems
                .Select(item => _mapper.Map<ToDoItemGetDto>(item))
                .ToList();

            var getAllResponseModel = new GetAllResponseModel()
            {
                ToDoItemGetDtos = toDoItemDtos,
                TotalCount = totalCount,
            };

            return getAllResponseModel;
        }

        public async Task<List<ToDoItemGetDto>> GetAllToDoItemsAsync(long userId)
        {
            var query = _toDoItemRepository.SelectAllToDoItems();
            query = query.Where(x => x.UserId == userId);
            var toDoItems = await query.ToListAsync();
            
            var toDoItemDtos = toDoItems
                .Select(item => _mapper.Map<ToDoItemGetDto>(item))
                .ToList();

            return toDoItemDtos;
        }

        public async Task<long> AddToDoItemAsync(ToDoItemCreateDto toDoItem)
        {
            var validationResult = _toDoItemCreateDtoValidator.Validate(toDoItem);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            ArgumentNullException.ThrowIfNull(toDoItem);
            var covert = _mapper.Map<ToDoItem>(toDoItem);

            var id = await _toDoItemRepository.InsertToDoItemAsync(covert);
            return id;
        }




        public async Task DeleteToDoItemByIdAsync(long id)
        {
            var item = await _toDoItemRepository.SelectToDoItemByIdAsync(id);
            if (item is null)
            {
                throw new NotFoundException($"ToDoItem with id {id} not found.");
            }
            await _toDoItemRepository.DeleteToDoItemByIdAsync(id);
        }

        public async Task<GetAllResponseModel> GetAllToDoItemsAsync(int skip, int take)
        {
            var toDoItems = await _toDoItemRepository.SelectAllToDoItemsAsync(skip, take);
            var totalCount = await _toDoItemRepository.SelectTotalCountAsync();
          

            var toDoItemDtos = toDoItems
                .Select(item => _mapper.Map<ToDoItemGetDto>(item))
                .ToList();

            var getAllResponseModel = new GetAllResponseModel()
            {
                ToDoItemGetDtos = toDoItemDtos,
                TotalCount = totalCount,
            };

            return getAllResponseModel;
        }


        public async Task<List<ToDoItemGetDto>> GetByDueDateAsync(DateTime dueDate)
        {
            var result = await _toDoItemRepository.SelectByDueDateAsync(dueDate);
            return result.Select(item => _mapper.Map<ToDoItemGetDto>(item)).ToList();
        }

        public async Task<GetAllResponseModel> GetCompletedAsync(int skip, int take)
        {
            var completedItems = await _toDoItemRepository.SelectCompletedAsync(skip, take);
            var totalCount = await _toDoItemRepository.SelectTotalCountAsync();

            var toDoItemDtos = completedItems
                       .Select(item => _mapper.Map<ToDoItemGetDto>(item))
                       .ToList();

            var getAllResponseModel = new GetAllResponseModel()
            {
                ToDoItemGetDtos = toDoItemDtos,
                TotalCount = totalCount,
            };

            return getAllResponseModel;
        }

        public async Task<GetAllResponseModel> GetIncompleteAsync(int skip, int take)
        {
            var incompleteItems = await _toDoItemRepository.SelectIncompleteAsync(skip, take);
            
            var totalCount = await _toDoItemRepository.SelectTotalCountAsync();
            var incompleteDtos = incompleteItems
                .Select(item => _mapper.Map<ToDoItemGetDto>(item))
                .ToList();

            var getAllResponseModel = new GetAllResponseModel()
            {
                ToDoItemGetDtos = incompleteDtos,
                TotalCount = totalCount,
            };

            return getAllResponseModel;
        }

        public async Task<ToDoItemGetDto> GetToDoItemByIdAsync(long id)
        {
            var founded = await _toDoItemRepository.SelectToDoItemByIdAsync(id);
            if (founded == null)
            {
                throw new NotFoundException($"ToDoItem with id {id} not found.");
            }
            return _mapper.Map<ToDoItemGetDto>(founded);
        }

        public async Task<int> GetTotalCountAsync()
        {
            return await _toDoItemRepository.SelectTotalCountAsync();
        }

        public async Task UpdateToDoItemAsync(ToDoItemUpdateDto newItem, long userId)
        {
            var validationResult = _toDoItemUpdateDtoValidator.Validate(newItem);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }
            var entity = _mapper.Map<ToDoItem>(newItem);
            entity.UserId = userId;
            await _toDoItemRepository.UpdateToDoItemAsync(entity);
        }

    }
}
