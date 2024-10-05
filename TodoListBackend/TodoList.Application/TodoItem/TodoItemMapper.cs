using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoList.Application.TodoItem
{
    public static class TodoItemMapper
    {
        public static Core.Entities.TodoItem ToModel(this TodoItemDto todoItemDto)
        {
            return new Core.Entities.TodoItem
            {
                Title = todoItemDto.Title,
                Description = todoItemDto.Description,
                Priority = todoItemDto.Priority,
                IsCompleted = todoItemDto.IsCompleted,
            };
        }
        public static TodoItemDto ToDto(this Core.Entities.TodoItem todoItem)
        {
            return new TodoItemDto
            {
                Title = todoItem.Title,
                Description = todoItem.Description,
                Priority = todoItem.Priority,
                IsCompleted = todoItem.IsCompleted
            };
        }
    }
}
