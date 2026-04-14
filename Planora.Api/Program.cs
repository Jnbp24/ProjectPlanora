using Microsoft.AspNetCore.Identity;
using Microsoft.DotNet.Scaffolding.Shared.ProjectModel;
using Planora.DataAccess;
using Microsoft.EntityFrameworkCore;
using Planora.Api.Services;
using Planora.Api.Services.Auth;
using Planora.DataAccess.Context;
using Planora.DataAccess.Models.Auth;
using Planora.Api.Services.User;
using Planora.Api.Services.Task;
using Planora.Api.Services.Category;
using Planora.Api.Services.Project;
using Planora.DataAccess.Repositories.Category;
using Planora.DataAccess.Repositories.Project;
using Planora.DataAccess.Repositories.Task;
using Planora.DataAccess.Repositories.User;

namespace Planora.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers(options =>
            {
                options.SuppressAsyncSuffixInActionNames = false;
            });
            builder.Services.AddDbContext<DbContext, DatabaseContext>(options => options.UseInMemoryDatabase("PlanoraDB"));
            
            //Service Layer
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<ITaskService, TaskService>();
            builder.Services.AddScoped<ICategoryService, CategoryService>();
            builder.Services.AddScoped<IProjectService, ProjectService>();

            //Repository
            builder.Services.AddScoped<ITaskRepository, TaskRepository>();
            builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IProjectRepository, ProjectRepository>();

            //Auth Services
            builder.Services.AddIdentity<AuthUser, IdentityRole>()
                .AddEntityFrameworkStores<DatabaseContext>();

            builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();
                
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            app.UseHttpsRedirection();

            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}
