using Microsoft.EntityFrameworkCore;
using System.Web.Http;
using TodoList.Infraestructure.Data;
using TodoList.Infraestructure.Repositories;
using Unity;
using Unity.Injection;
using Unity.Lifetime;
using Unity.WebApi;

namespace TodoList.WebAPI
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // e.g. container.RegisterType<ITestService, TestService>();

            container.RegisterFactory<ApplicationDbContext>(c =>
            {
                var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
                optionsBuilder.UseSqlite("Data Source=todolist.db");

                return new ApplicationDbContext(optionsBuilder.Options);
            });

            container.RegisterType<ITodoRepository, TodoRepository>();

            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}