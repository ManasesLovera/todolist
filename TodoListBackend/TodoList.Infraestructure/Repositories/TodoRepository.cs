using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoList.Core.Entities;
using TodoList.Infraestructure.Data;

namespace TodoList.Infraestructure.Repositories
{
    public class TodoRepository : ITodoRepository
    {
        private readonly ApplicationDbContext _context;
        public TodoRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(TodoItem item)
        {
            var todoItem = await _context.TodoItems.FindAsync(item.Id);

            if (todoItem != null)
                return;

            await _context.TodoItems.AddAsync(item);
            await _context.SaveChangesAsync();
        }

        public async Task ChangeCompletedAsync(int id)
        {
            var todoItem = await _context.TodoItems.FindAsync(id);

            if (todoItem == null)
                return;

            todoItem.IsCompleted = !todoItem.IsCompleted;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var todoItem = await _context.TodoItems.FindAsync(id);

            if (todoItem == null)
                return;

            _context.Remove(todoItem);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<TodoItem>> GetAllAsync()
        {
            return await _context.TodoItems.ToListAsync();
        }

        public async Task<TodoItem> GetByIdAsync(int id)
        {
            return await _context.TodoItems.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<TodoItem> GetByTitleAsync(string title)
        {
            return await _context.TodoItems.FirstOrDefaultAsync(x => x.Title == title);
        }

        public async Task UpdateAsync(TodoItem item)
        {
            var todoItem = await _context.TodoItems.FindAsync(item.Id);

            if (todoItem == null)
                return;

            todoItem.Title = item.Title;
            todoItem.Description = item.Description;
            todoItem.Priority = item.Priority;
            todoItem.IsCompleted = item.IsCompleted;

            await _context.SaveChangesAsync();
        }
    }
}
