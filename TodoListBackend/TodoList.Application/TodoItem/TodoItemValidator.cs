using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoList.Core.Enums;

namespace TodoList.Application.TodoItem
{
    public static class TodoItemValidator
    {
        public static ValidateResponse Validate(this TodoItemRequestDto todoItemDto)
        {
            List<string> errors = new List<string>();

            if (String.IsNullOrWhiteSpace(todoItemDto.Title))
                errors.Add("Title is missing");
            else if (todoItemDto.Title.Length < 5 && todoItemDto.Title.Length >= 25)
                errors.Add("Title is not valid, it must be over 5 and under 25 characters");

            if (String.IsNullOrWhiteSpace(todoItemDto.Description))
                errors.Add("Title is missing");
            else if (todoItemDto.Description.Length < 5 && todoItemDto.Description.Length >= 50)
                errors.Add("Title is not valid, it must be over 5 and under 50 characters");

            if (String.IsNullOrWhiteSpace(todoItemDto.Priority))
                errors.Add("Priority can't be null, empty, or only whitespace");
            else if (! new List<string>() { "High", "Medium", "Low" }.Contains(todoItemDto.Priority))
                errors.Add("Priority is not valid, it most be one of these: High, Medium, Low");            
            
            return new ValidateResponse()
            {
                IsValid = errors.Count == 0,
                Errors = errors
            };
        }
    }
    public class ValidateResponse
    {
        public bool IsValid { get; set; }
        public List<string> Errors { get; set; }
    }
}
