using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using TodoApp.Configurations.Validation;
using TodoApp.DataAccess.QuerySpecs;
using TodoApp.Domain.Entities;
using TodoApp.Models.Common;
using TodoApp.Models.Pagination;
using TodoApp.Models.Todo;
using TodoApp.Services.Interfaces.TodoService;
using TodoApp.Utiities.Exceptions;

namespace TodoApp.Controllers;

[Route("api/todos/")]
[ApiController]
public class TodoController : BaseController
{
    private readonly ITodoService _todoService;
    private readonly IMediator _validator;
    private readonly IMapper _mapper;
    public TodoController(ITodoService todoService, IMediator validator, IMapper mapper)
    {
        _todoService = todoService;
        _validator = validator;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<PagedResponse<Todo>>>> GetAllTodos(TodoSpec spec)
    {
        // Ensure only the current user's todos are returned
        if (spec.Filters == null)
            spec.Filters = new Dictionary<string, string>();

        spec.Filters["UserId"] = userId;

        var todos = await _todoService.GetPaginatedListAsync(spec);
        return Ok(todos);
    }

    [HttpGet("{id}")]
    private async Task<ActionResult<ApiResponse<Todo>>> GetTodoById(Guid id)
    {
        Todo? todo = await _todoService.GetByIdAsync(id);
        return todo is null ? throw new NotFoundException("Todo not found") : Ok(new ApiResponse<Todo>(todo));
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<string>>> CreateTodo([FromBody] TodoCreateDto model)
    {
        var validationResult = await _validator.ValidateAsync(model);
        if (!validationResult.IsValid) throw new ValidationException(validationResult.Errors);

        var todo = _mapper.Map<Todo>(model);
        todo.UserId = userId;

        bool isCreated = await _todoService.AddAsync(todo);

        return isCreated ? Ok(new ApiResponse<string>("Created Successfully"))
                         : throw new BadRequestException("Failed to create todo");
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse<Todo>>> UpdateTodo(Guid id, [FromBody] TodoUpdateDto model)
    {
        var validationResult = await _validator.ValidateAsync(model);
        if (!validationResult.IsValid) throw new ValidationException(validationResult.Errors);

        Todo todo = await _todoService.GetByIdAsync(id);
        _mapper.Map(model, todo);

        bool isUpdated = await _todoService.UpdateAsync(todo);

        return isUpdated ? Ok(new ApiResponse<string>("Updated Successfully"))
                         : throw new BadRequestException("Failed to Update todo");
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse<string>>> DeleteTodo(Guid id)
    {
        bool isDeleted = await _todoService.DeleteAsync(id);
        return isDeleted ? Ok(new ApiResponse<string>("Deleted Successfully")) : throw new NotFoundException("Todo not found");
    }
}
