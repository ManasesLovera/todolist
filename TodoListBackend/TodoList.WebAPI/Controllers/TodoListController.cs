using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Remoting.Messaging;
using System.Threading.Tasks;
using System.Web.Http;
using TodoList.Application.TodoItem;
using TodoList.Core.Entities;
using TodoList.Infraestructure.Repositories;

namespace TodoList.WebAPI.Controllers
{
    [RoutePrefix("api/todolist")]
    public class TodoListController : ApiController
    {
        private readonly ITodoRepository _todoRepo;
        public TodoListController(ITodoRepository todoRepository)
        {
            _todoRepo = todoRepository;
        }
        // GET api/todolist
        [HttpGet]
        [Route("")]
        public async Task<IEnumerable<TodoItem>> GetAll()
        {
            return await _todoRepo.GetAllAsync();
        }

        // GET api/todolist/{id}
        [HttpGet]
        [Route("{id:int}")]
        public async Task<IHttpActionResult> Get(int id)
        {
            var item = await _todoRepo.GetByIdAsync(id);

            if (item == null)
                return NotFound();
            
            return Ok(item);
        }

        // POST api/todolist
        [HttpPost]
        [Route("")]
        public async Task<HttpResponseMessage> Post([FromBody] TodoItemDto itemDto)
        {
            var validationResult = itemDto.Validate();

            if (!validationResult.IsValid)
                return Request.CreateResponse(HttpStatusCode.BadRequest, validationResult.Errors);

            var todoItem = await _todoRepo.GetByTitleAsync(itemDto.Title);

            if (todoItem != null)
                return Request.CreateResponse(HttpStatusCode.Conflict, new { Message = "There is an TodoItem with this title already." });

            await _todoRepo.AddAsync(itemDto.ToModel());
            var newTodoItem = await _todoRepo.GetByTitleAsync(itemDto.Title);

            return Request.CreateResponse(HttpStatusCode.Created, "TodoItem created successfully");
        }

        // PUT api/todolist/{id}
        [HttpPut]
        [Route("{id}")]
        public async Task<IHttpActionResult> Put(int id, [FromBody] TodoItemDto itemDto)
        {
            var todoItem = await _todoRepo.GetByIdAsync(id);

            if (todoItem == null)
                return NotFound();

            var updatedItem = itemDto.ToModel();
            updatedItem.Id = id;

            await _todoRepo.UpdateAsync(updatedItem);
            return Ok(updatedItem);
        }

        // DELETE api/todolist/{id}
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
    }
}