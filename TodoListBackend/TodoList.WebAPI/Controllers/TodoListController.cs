using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Remoting.Messaging;
using System.Threading.Tasks;
using System.Web.Http;
using TodoList.Application.TodoItem;
using TodoList.Application.TodoItem.DTOs;
using TodoList.Core.Entities;
using TodoList.Infraestructure.Repositories;

namespace TodoList.WebAPI.Controllers
{
    [RoutePrefix("tasks")]
    public class TodoListController : ApiController
    {
        private readonly ITodoRepository _todoRepo;
        public TodoListController(ITodoRepository todoRepository)
        {
            _todoRepo = todoRepository;
        }
        // GET tasks
        [HttpGet]
        [Route("")]
        public async Task<IEnumerable<TodoItemResponseDto>> GetAll()
        {
            var todoItems = await _todoRepo.GetAllAsync();
            List<TodoItemResponseDto> todoItemsResponse = todoItems.Select(i => i.ToResponseDto()).ToList();
            return todoItemsResponse;
        }

        // GET tasks/{id}
        [HttpGet]
        [Route("{id:int}")]
        public async Task<IHttpActionResult> Get(int id)
        {
            var item = await _todoRepo.GetByIdAsync(id);

            if (item == null)
                return NotFound();
            
            return Ok(item.ToResponseDto());
        }

        // POST tasks
        [HttpPost]
        [Route("")]
        public async Task<HttpResponseMessage> Post([FromBody] TodoItemRequestDto itemDto)
        {
            if (itemDto == null)
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Invalid request body");

            var validationResult = itemDto.Validate();

            if (!validationResult.IsValid)
                return Request.CreateResponse(HttpStatusCode.BadRequest, validationResult.Errors);

            var todoItem = await _todoRepo.GetByTitleAsync(itemDto.Title);

            if (todoItem != null)
                return Request.CreateResponse(HttpStatusCode.Conflict, new { Message = "There is an TodoItem with this title already." });

            await _todoRepo.AddAsync(itemDto.ToModel());
            var newTodoItem = await _todoRepo.GetByTitleAsync(itemDto.Title);

            return Request.CreateResponse(HttpStatusCode.Created, newTodoItem);
        }
        // PUT tasks/{id}
        [HttpPut]
        [Route("{id}")]
        public async Task<IHttpActionResult> Put(int id, [FromBody] TodoItemRequestDto itemDto)
        {
            var todoItem = await _todoRepo.GetByIdAsync(id);

            if (todoItem == null)
                return NotFound();

            var updatedItem = itemDto.ToModel();
            updatedItem.Id = id;

            await _todoRepo.UpdateAsync(updatedItem);
            return Ok(updatedItem.ToResponseDto());
        }
        // DELETE tasks/{id}
        [HttpDelete]
        [Route("{id}")]
        public async Task<IHttpActionResult> Delete(int id)
        {
            var todoItem = await _todoRepo.GetByIdAsync(id);
            if (todoItem == null)
                return NotFound();

            await _todoRepo.DeleteAsync(id);
            return StatusCode(HttpStatusCode.NoContent);
        }
        // PATCH tasks/{id}
        [HttpPatch]
        [Route("{id}")]
        public async Task<IHttpActionResult> ChangeCompleted(int id)
        {
            var todoItem = await _todoRepo.GetByIdAsync(id);

            if (todoItem == null)
                return NotFound();

            await _todoRepo.ChangeCompletedAsync(id);
            return Ok(todoItem.ToResponseDto());
        }
    }
}