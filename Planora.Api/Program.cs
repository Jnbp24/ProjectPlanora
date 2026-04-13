using Microsoft.AspNetCore.Identity;
using Planora.DataAccess;
using Microsoft.EntityFrameworkCore;
using Planora.Api.Services.Auth;
using Planora.DataAccess.Context;
using Planora.DataAccess.Models.Auth;

namespace Planora.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddDbContext<DbContext,DatabaseContext>(options => options.UseInMemoryDatabase("PlanoraDB"));
            
            //Service Layer
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<ITaskService, TaskService>();
            builder.Services.AddScoped<ICategoryService, CategoryService>();

            //Auth Services
            builder.Services.AddIdentity<AuthUser, IdentityRole>()
                .AddEntityFrameworkStores<DatabaseContext>();

            builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();
                
            var app = builder.Build();

            // Configure the HTTP request pipeline.

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();
            app.MapStaticAssets();

            app.Run();
        }
    }
}
