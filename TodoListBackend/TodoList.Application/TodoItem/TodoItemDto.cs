using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoList.Core.Enums;

namespace TodoList.Application.TodoItem
{
    public class TodoItemDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public Priority Priority { get; set; }
        public bool IsCompleted { get; set; }
    }
}
