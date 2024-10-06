using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoList.Application.TodoItem.DTOs;
using TodoList.Core.Enums;

namespace TodoList.Application.TodoItem
{
    public static class TodoItemMapper
    {
        public static Core.Entities.TodoItem ToModel(this TodoItemRequestDto todoItemDto)
        {
            Priority myPriority = Priority.Low;
            if (new List<string>() { "High", "Medium", "Low" }.Contains(todoItemDto.Priority))
            {
                myPriority = (Priority) Enum.Parse(typeof(Priority), todoItemDto.Priority);
            }

            return new Core.Entities.TodoItem
            {
                Title = todoItemDto.Title,
                Description = todoItemDto.Description,
                Priority = myPriority,
                IsCompleted = todoItemDto.IsCompleted
            };
        }
        public static TodoItemRequestDto ToRequestDto(this Core.Entities.TodoItem todoItem)
        {
            return new TodoItemRequestDto
            {
                Title = todoItem.Title,
                Description = todoItem.Description,
                Priority = todoItem.Priority.ToString(),
                IsCompleted = todoItem.IsCompleted
            };
        }
        public static TodoItemResponseDto ToResponseDto(this Core.Entities.TodoItem todoItem)
        {
            return new TodoItemResponseDto
            {
                Id = todoItem.Id,
                Title = todoItem.Title,
                Description = todoItem.Description,
                Priority = todoItem.Priority.ToString(),
                IsCompleted = todoItem.IsCompleted
            };
        }
    }
}
