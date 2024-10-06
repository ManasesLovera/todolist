using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http;
using System.Web.Http.Results;
using TodoList.Application.TodoItem;
using TodoList.Application.TodoItem.DTOs;
using TodoList.Core.Entities;
using TodoList.Core.Enums;
using TodoList.Infraestructure.Repositories;
using TodoList.WebAPI.Controllers;

namespace TodoList.Tests
{
    [TestFixture]
    public class TodoListControllerTests
    {
        private TodoListController _controller;
        private Mock<ITodoRepository> _mockRepo;

        [SetUp]
        public void SetUp()
        {
            _mockRepo = new Mock<ITodoRepository>();
            _controller = new TodoListController(_mockRepo.Object);

            //var httpRequestMessage = new HttpRequestMessage();
            //var httpConfiguration = new HttpConfiguration();
            //var controllerContext = new HttpControllerContext
            //{
            //    Request = httpRequestMessage,
            //    Configuration = httpConfiguration
            //};

            //_controller.ControllerContext = controllerContext;
        }
        // GET api/todolist
        [Test]
        public async Task GetAll_ReturnsTodoItems()
        {
            var todoItems = new List<TodoItem>
            {
                new TodoItem { Id = 1, Title = "titulo", Description = "description", Priority = Priority.High, IsCompleted = false },
                new TodoItem { Id = 2, Title = "titulo2", Description = "description2", Priority = Priority.Medium, IsCompleted = true }
            };

            _mockRepo.Setup(repo => repo.GetAllAsync()).ReturnsAsync(todoItems);

            var result = await _controller.GetAll();

            Assert.That(2, Is.EqualTo(result.Count() ));
            Assert.That("titulo", Is.EqualTo(result.First().Title));
        }
        // GET api/todolist/{id}
        [Test]
        public async Task Get_ReturnTodoItem_WhenItemExists()
        {
            var todoItem = new TodoItem { Id = 1 , Title = "titulo", Description = "Description", Priority = Priority.Low, IsCompleted = false };
            _mockRepo.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(todoItem);

            var result = await _controller.Get(1);

            Assert.That(result, Is.InstanceOf<OkNegotiatedContentResult<TodoItemResponseDto>>());
            var contentResult = (OkNegotiatedContentResult<TodoItemResponseDto>) result;
            Assert.That(contentResult.Content.Title, Is.EqualTo("titulo"));
        }
        [Test]
        public async Task Get_ReturnsNotFound_WhenItemDoesNotExist()
        {
            _mockRepo.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync((TodoItem)null);

            var result = await _controller.Get(1);

            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }
        // POST api/todolist
        [Test]
        public async Task Post_CreatesTodoItem_WhenValid()
        {
            var todoItem = new TodoItem { Id = 1, Title = "titulo", Description = "description", Priority = Priority.High, IsCompleted = false };
            
            _mockRepo.SetupSequence(repo => repo.GetByTitleAsync(todoItem.Title))
                .ReturnsAsync((TodoItem)null)
                .ReturnsAsync(todoItem);

            _mockRepo.Setup(repo => repo.AddAsync(It.IsAny<TodoItem>())).Returns(Task.CompletedTask);
            //_mockRepo.Setup(repo => repo.GetByTitleAsync(itemDto.Title)).ReturnsAsync(new TodoItem { 
            //    Id = 1, 
            //    Title = "titulo", 
            //    Description = "description",
            //    Priority = Priority.High, 
            //    IsCompleted = false 
            //});

            _controller.Request = new HttpRequestMessage();
            _controller.Request.SetConfiguration(new HttpConfiguration());

            var response = await _controller.Post(todoItem.ToRequestDto());

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));
        }
        [Test]
        public async Task Post_ReturnsBadRequest_WhenInvalid()
        {
            var itemDto = new TodoItemRequestDto { Title = "", Description = "description", Priority = "High", IsCompleted = false };
            var validationResult = itemDto.Validate();

            _controller.Request = new HttpRequestMessage();
            _controller.Request.SetConfiguration(new HttpConfiguration());

            var response = await _controller.Post(itemDto);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [Test]
        public async Task Post_ReturnsConflict_WhenTitleAlreadyExists()
        {
            var todoItem = new TodoItem { Id = 1, Title = "titulo", Description = "description", Priority = Priority.High, IsCompleted = false };
            _mockRepo.Setup(repo => repo.GetByTitleAsync(todoItem.Title)).ReturnsAsync(todoItem);

            _controller.Request = new HttpRequestMessage();
            _controller.Request.SetConfiguration(new HttpConfiguration());

            var response = await _controller.Post(todoItem.ToRequestDto());

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Conflict));
        }
        // PUT api/todolist/{id}
        [Test]
        public async Task Put_UpdatesTodoItem_WhenItemExists()
        {
            var todoItem = new TodoItem { Id = 1, Title = "titulo", Description = "description", Priority = Priority.High, IsCompleted = false };
            var updatedDto = new TodoItemRequestDto { Title = "new title", Description = "new description", Priority = "Medium", IsCompleted = true };

            _mockRepo.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(todoItem);
            _mockRepo.Setup(repo => repo.UpdateAsync(It.IsAny<TodoItem>())).Returns(Task.CompletedTask);

            var result = await _controller.Put(1, updatedDto);

            Assert.That(result, Is.InstanceOf<OkNegotiatedContentResult<TodoItemResponseDto>>());
        }

        [Test]
        public async Task Put_ReturnsNotFound_WhenItemDoesNotExist()
        {
            var updatedDto = new TodoItemRequestDto { Title = "new title", Description = "new description", Priority = "Medium", IsCompleted = true };
            _mockRepo.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync((TodoItem)null); 

            var result = await _controller.Put(1, updatedDto);

            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }
        // DELETE api/todolist/{id}
        [Test]
        public async Task Delete_RemovesTodoItem_WhenItemExists()
        {
            var todoItem = new TodoItem { Id = 1, Title = "titulo", Description = "description", Priority = Priority.High, IsCompleted = false };
            _mockRepo.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(todoItem);
            _mockRepo.Setup(repo => repo.DeleteAsync(1)).Returns(Task.CompletedTask);

            var result = await _controller.Delete(1);

            Assert.That(result, Is.InstanceOf<StatusCodeResult>());
            var statusCodeResult = (StatusCodeResult)result;
            Assert.That(statusCodeResult.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
        }

        [Test]
        public async Task Delete_ReturnsNotFound_WhenItemDoesNotExist()
        {
            _mockRepo.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync((TodoItem)null);

            var result = await _controller.Delete(1);

            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }
        // PATCH api/todolist
        [Test]
        public async Task ChangeCompleted_ReturnsOk_WhenItemExists()
        {
            var todoItem = new TodoItem { Id = 1, Title = "titulo", Description = "description", Priority = Priority.High, IsCompleted = false };
            _mockRepo.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(todoItem);
            _mockRepo.Setup(repo => repo.ChangeCompletedAsync(1)).Returns(Task.CompletedTask);

            var result = await _controller.ChangeCompleted(1);

            Assert.That(result, Is.InstanceOf<OkNegotiatedContentResult<TodoItemResponseDto>>());
        }

        [Test]
        public async Task ChangeCompleted_ReturnsNotFound_WhenItemDoesNotExist()
        {
            _mockRepo.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync((TodoItem)null);

            var result = await _controller.ChangeCompleted(1);

            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }
    }
}
