using Microsoft.AspNetCore.Identity;
using Planora.DataAccess;
using Microsoft.EntityFrameworkCore;
using Planora.Api.Services.Auth;
using Planora.DataAccess.Context;
using Planora.DataAccess.Models.Auth;
using Planora.Api.Controllers;
using Planora.Api.Services;
using Planora.DataAccess.Repositories.User;

namespace Planora.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();

			builder.Services.AddScoped<UserRepository>();
			builder.Services.AddScoped<UserService>();

			builder.Services.AddDbContext<DbContext,DatabaseContext>(options => options.UseInMemoryDatabase("PlanoraDB"));

            //Auth Services
            builder.Services.AddIdentity<AuthUser, IdentityRole>()
                .AddEntityFrameworkStores<DatabaseContext>();

            builder.Services.AddScoped<JwtTokenService>();
                
            var app = builder.Build();

            // Configure the HTTP request pipeline.

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
